// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Xamarin_ios_language_manager
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel bottomLeftLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel bottomRightLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton imagePickerButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tableView { get; set; }

        [Action ("imagePickerButton:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void imagePickerButtonTouchUp (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (bottomLeftLabel != null) {
                bottomLeftLabel.Dispose ();
                bottomLeftLabel = null;
            }

            if (bottomRightLabel != null) {
                bottomRightLabel.Dispose ();
                bottomRightLabel = null;
            }

            if (imagePickerButton != null) {
                imagePickerButton.Dispose ();
                imagePickerButton = null;
            }

            if (tableView != null) {
                tableView.Dispose ();
                tableView = null;
            }
        }
    }
}