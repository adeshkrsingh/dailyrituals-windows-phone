using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace DailyRituals
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class AddRitual : Page
  {
    public AddRitual()
    {
      this.InitializeComponent();
    }

    /// <summary>
    /// Invoked when this page is about to be displayed in a Frame.
    /// </summary>
    /// <param name="e">Event data that describes how this page was reached.
    /// This parameter is typically used to configure the page.</param>
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
    }

    private void addButton_Click(object sender, RoutedEventArgs e)
    {
        if(maxdays.Text.Equals(""))
        {
            maxdays.Text = "0";
        }
        int x = Int32.Parse(maxdays.Text);
      // Save this new ritual
        if( x== 0)
        {
            x = 7;
        }
      App.DataModel.AddRitual(goalNameTextBox.Text, goalDescriptionTextBox.Text, x);

      Frame.Navigate(typeof(MainPage));
    }

    private async void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
    {
        MessageDialog msgbox = new MessageDialog("please input correct value");
        await msgbox.ShowAsync();
    }

  }
}
