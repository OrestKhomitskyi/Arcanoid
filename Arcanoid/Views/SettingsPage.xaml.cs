using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Arcanoid.Properties;

namespace Arcanoid.Views
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public event Action<bool?> ToggleMusic;
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            ToggleMusic(cb.IsChecked);
            Settings.Default.SoundOn = (bool)cb.IsChecked;
            Settings.Default.Save();
            MessageBox.Show(Settings.Default.SoundOn.ToString());
        }
    }
}
