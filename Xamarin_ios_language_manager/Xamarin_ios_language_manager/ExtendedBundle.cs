using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UIKit;
using Foundation;
using ObjCRuntime;

namespace Xamarin_ios_language_manager
{
    public class ExtendedBundle : NSBundle
    {

		public ExtendedBundle(IntPtr handle) : base(handle) {
		}

		static NSString bundleKey = new NSString("0");

        [Export("localizedStringForKey:value:table:")]
        public override string LocalizedString(string key, string value, string table)
        {
            Debug.WriteLine($"So we hit LocalizedString = {bundleKey}");
            NSBundle bundle = this.GetProperty<NSBundle>(bundleKey);
            //Debug.WriteLine("So we hit LocalizedString");
            if (bundle != null)
            {
                Debug.WriteLine($"Bundle !null {key} {value} {table}");
                return bundle.LocalizedString(key, value, table);
            }
            else
            {
                Debug.WriteLine($"Bundle null {key} {value} {table}");
                return base.LocalizedString(key, value, table);
            }
        }
    }

    [Category(typeof(NSBundle))]
    public static class NSBundleExtensions {
        
        static NSString bundleKey = new NSString("0");

        [Export("setLanguage:")]
		public static void SetLanguage(this NSBundle bundle, string currentLanguage)
		{
			try
			{
				//static dispatch_once_t onceToken;
				//dispatch_once(&onceToken, ^{
				var intPtr = NSBundleAssociatedObjectExtension.object_setClass(NSBundle.MainBundle.Handle, new Class(typeof(ExtendedBundle)).Handle);

                var swapBundle = (NSBundle)ObjCRuntime.Runtime.GetNSObject(intPtr);

                if (LanguageManager.IsCurrentLanguageRTL())
                {
                    //Debug.WriteLine($"SetLanguage is RTL {LanguageManager.IsCurrentLanguageRTL()}");
                    if (new UIView().RespondsToSelector(new Selector("setSemanticContentAttribute:")))
                    {
                        NSBundleAssociatedObjectExtension.SetLanguageRTL();
                    }
                }
                else
                {
                    //Debug.WriteLine($"SetLanguage is LTR {LanguageManager.IsCurrentLanguageRTL()}");
					if (new UIView().RespondsToSelector(new Selector("setSemanticContentAttribute:")))
					{
						NSBundleAssociatedObjectExtension.SetLanguageLTR();
					}
                }
                NSUserDefaults.StandardUserDefaults.SetBool(LanguageManager.IsCurrentLanguageRTL(), "AppleTextDirection");
                NSUserDefaults.StandardUserDefaults.SetBool(LanguageManager.IsCurrentLanguageRTL(), "NSForceRightToLeftWritingDirection");
                NSUserDefaults.StandardUserDefaults.Synchronize();
                

				var returnBundle = NSBundle.FromPath(NSBundle.MainBundle.PathForResource(currentLanguage, "lproj"));
				NSBundle.MainBundle.SetProperty(bundleKey, returnBundle, NSBundleAssociatedObjectExtension.AssociationPolicy.RetainNonAtomic);
             
                //Debug.WriteLine("SetLanguage has been called");

			}
			catch (Exception ex)
			{
				throw (new Exception(ex.Message));
			}
		}
    }

    public static class NSBundleAssociatedObjectExtension
    {
        public enum AssociationPolicy
        {
            Assign = 0,
            RetainNonAtomic = 1,
            CopyNonAtomic = 3,
            Retain = 01401,
            Copy = 01403,
        }

		[DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
		internal extern static IntPtr IntPtr_objc_msgSend(IntPtr receiver, IntPtr selector, UISemanticContentAttribute arg1);

		public static void SetLanguageLTR()
		{
			Selector selector = new Selector("setSemanticContentAttribute:");
			IntPtr_objc_msgSend(UIView.Appearance.Handle, selector.Handle, UISemanticContentAttribute.ForceLeftToRight);
		}

		public static void SetLanguageRTL()
		{
			Selector selector = new Selector("setSemanticContentAttribute:");
            IntPtr_objc_msgSend(UIView.Appearance.Handle, selector.Handle, UISemanticContentAttribute.ForceRightToLeft);
		}

        [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "object_getClass")]
        internal static extern IntPtr object_getClass(IntPtr obj);

        [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "object_setClass")]
        internal static extern IntPtr object_setClass(IntPtr obj, IntPtr cls);

		[DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_setAssociatedObject")]
        internal static extern void objc_setAssociatedObject(IntPtr pointer, IntPtr key, IntPtr value, AssociationPolicy policy);

        [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_getAssociatedObject")]
        internal static extern IntPtr objc_getAssociatedObject(IntPtr pointer, IntPtr key);

        public static T GetProperty<T>(this NSBundle bundle, NSString propertyKey) where T : NSObject
        {
            var pointer = objc_getAssociatedObject(bundle.Handle, propertyKey.Handle);
            return Runtime.GetNSObject<T>(pointer);
        }

        public static void SetProperty<T>(this NSBundle bundle, NSString propertyKey, T value, AssociationPolicy policy) where T : NSObject
        {
            objc_setAssociatedObject(bundle.Handle, propertyKey.Handle, value.Handle, policy);
        }
    }
}
