using System;
using Foundation;
using UIKit;

//http://stackoverflow.com/questions/21940340/how-to-use-objc-setassociatedobject-in-monotouch
//https://www.factorialcomplexity.com/blog/2015/01/28/how-to-change-localization-internally-in-your-ios-application.html
//https://github.com/maximbilan/ios_language_manager
//https://developer.xamarin.com/guides/ios/advanced_topics/localization_and_internationalization/#code

namespace Xamarin_ios_language_manager
{
    public partial class ViewController : UIViewController, IUIImagePickerControllerDelegate
    {

        UITableViewSource dataSource;
        UIImagePickerController imagePickerController;

		protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            bottomLeftLabel.Text = "Happy New Year";
			bottomRightLabel.Text = "ПТНПНХ";

            dataSource = new CustomDataSource(this);
            tableView.Source = dataSource;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

			bottomLeftLabel.Text = NSBundle.MainBundle.LocalizedString("Happy New Year", "ReplacedText");
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

		void ReloadRootViewController(){
            var appDelegate = UIApplication.SharedApplication.Delegate;
            appDelegate.GetWindow().RootViewController = this.Storyboard.InstantiateInitialViewController();
		}

        partial void imagePickerButtonTouchUp (UIKit.UIButton sender){
            UIImagePickerController imagePickerController = new UIImagePickerController();
            imagePickerController.ModalPresentationStyle = UIModalPresentationStyle.CurrentContext;
            imagePickerController.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
            imagePickerController.Delegate = this;

			this.imagePickerController = imagePickerController;
            PresentViewController(this.imagePickerController, true, null);
        }

        [Export("imagePickerController:didFinishPickingMediaWithInfo:")]
        public void FinishedPickingMedia(UIImagePickerController picker, NSDictionary info)
        {
            DismissViewController(true, null);
            imagePickerController.Dispose();
        }

        [Export("imagePickerControllerDidCancel:")]
        public void Canceled(UIImagePickerController picker)
        {
            DismissViewController(true, null);
        }

        private class CustomDataSource : UITableViewSource
        {
            const string cellIdentifier = "cellIdentifier";
            LanguageManager languageManager;
            ViewController controller;

            public CustomDataSource(ViewController controller)
            {
                this.controller = controller;
                controller.tableView.RegisterClassForCellReuse(typeof(UITableViewCell), cellIdentifier);
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
                cell.TextLabel.Text = LanguageManager.LanguageStrings[indexPath.Row];
			    if (indexPath.Row == LanguageManager.CurrentLanguageIndex) {
                    cell.Accessory = UITableViewCellAccessory.Checkmark;
			    }
			    else {
			        cell.Accessory = UITableViewCellAccessory.None;
			    }
			    return cell;
            }

            public override nint RowsInSection(UITableView tableView, nint section)
            {
                return LanguageManager.LanguageStrings.Count;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                //languageManager.CurrentLanguageIndex = indexPath.Row;
                LanguageManager.SaveLanguageByIndex(indexPath.Row);
                tableView.ReloadData();
                controller.ReloadRootViewController();
            }
        }
    }
}
