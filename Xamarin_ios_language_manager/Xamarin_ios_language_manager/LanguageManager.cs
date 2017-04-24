using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using ObjCRuntime;
//using System.Diagnostics;

namespace Xamarin_ios_language_manager
{
    public class LanguageManager
    {
        //string currentLanguageString;
        string currentLanguageCode;

        static List<string> LanguageCodes = new List<string> { "en", "de", "fr", "ar" };
        public static List<string> LanguageStrings = new List<string> { "English", "German", "French", "Arabic" };
        public static string LanguageSaveKey = "currentLanguageKey";

        public LanguageManager()
        {
            currentLanguageCode = NSUserDefaults.StandardUserDefaults.StringForKey(LanguageSaveKey);
        }

        static int currentLanguageIndex = 0;
        public static int CurrentLanguageIndex
        {
            get => currentLanguageIndex;
            set => currentLanguageIndex = value;
        }

        //static bool isCurrentLanguageRTL;
        public static bool IsCurrentLanguageRTL() {
            return NSLocale.GetCharacterDirection(LanguageCodes[currentLanguageIndex]) == NSLocaleLanguageDirection.RightToLeft;
        }

        public static void SetupCurrentLanguage()
        {
            string currentLanguage = NSUserDefaults.StandardUserDefaults.StringForKey(LanguageSaveKey);
            if (string.IsNullOrEmpty(currentLanguage))
            {
                Array languages = NSUserDefaults.StandardUserDefaults.ArrayForKey("AppleLanguages");

                if (languages.Length > 0)
                {
                    currentLanguage = languages.GetValue(0).ToString();
                }
                NSUserDefaults.StandardUserDefaults.SetString(currentLanguage, LanguageSaveKey);
                NSUserDefaults.StandardUserDefaults.Synchronize();
            }

            if (!string.IsNullOrEmpty(currentLanguage))
            {
                
                //Debug.WriteLine($"Going to set setLanguage {currentLanguage}");
                //UIApplication.SharedApplication.SendAction(new Selector("setLanguage:"), target: NSBundle.MainBundle, sender: new NSString(currentLanguage), forEvent: null);
                if (NSBundle.MainBundle.RespondsToSelector(new Selector("setLanguage:"))){
                    if (UIApplication.SharedApplication != null)
                    {
                        UIApplication.SharedApplication.SendAction(new Selector("setLanguage:"), target: NSBundle.MainBundle, sender: new NSString(currentLanguage), forEvent: null);
                    }
                    else
                    {
                        NSBundle.MainBundle.PerformSelector(new Selector("setLanguage:"), new NSString(currentLanguage), 1);
                    }
                    //Debug.WriteLine("Should have set setLanguage");
                }
            }
        }

        public static void SaveLanguageByIndex(int index)
        {
            if (index >= 0 && index < LanguageStrings.Count)
            {
                string code = LanguageCodes[index];
                LanguageManager.CurrentLanguageIndex = index;
                NSUserDefaults.StandardUserDefaults.SetValueForKey(new NSString(code), new NSString(LanguageManager.LanguageSaveKey));
                NSUserDefaults.StandardUserDefaults.Synchronize(); 


                LanguageManager.SetupCurrentLanguage();
				//#ifdef USE_ON_FLY_LOCALIZATION
				//        [NSBundle setLanguage:code];
				//#endif
            }
        }

    }
}
