using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Karaoke
{
	public partial class MainWindow : Window
	{
		private readonly string projectMusicPath;
		private List<Song> currentSongList;
		private System.Diagnostics.Process activeProcess;
		private int currentIndex = -1;

		public MainWindow()
		{
			InitializeComponent();
			CheckFileSystem();
			projectMusicPath = "music";
			SetUI(0);
		}

		private void CheckFileSystem()
		{
			foreach (var l in Song.languages)
			{
				Directory.CreateDirectory(@"music\" + l);
			}
		}

		private List<Song> GetMusicList(int index)
		 {
			var language = Song.languages[index];
			IEnumerable<string> musics;
			
			if (index == 0 || index == 2 || index == 7 || index == 8)
			{
				//turkish, child, turkish archive and russian
				musics = Directory.GetFiles(projectMusicPath + "/" + language, "*.*", SearchOption.AllDirectories);
			}
			else
			{
				//foreign
				musics = Directory.GetFiles(projectMusicPath + "/" + language, "*.*", SearchOption.AllDirectories).Where(name => !name.EndsWith(".mp3"));
			}
			
			var songlist = new List<Song>();

			foreach (var music in musics)
			{
				songlist.Add(new Song()
				{
					FilePath = music,
					Language = Song.languages[index]
				});
			}

			return songlist;
		}
		

		private void _searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			var searchString = _searchTextBox.Text.Trim();
			var filteredList = currentSongList.Where(song => song.ToString().ToLower().Contains(searchString.ToLower()));
			_listView.ItemsSource = filteredList;
		}

		private void ListViewItemDoubleClicked(object sender, MouseButtonEventArgs e)
		{
			Song selectedSong = _listView.SelectedItem as Song;
			string selectedSongFilePath = "\"" + selectedSong.FilePath + "\"";
			System.Diagnostics.Process playerProcess;

			try
			{
				if(selectedSong.Language.Equals("turkce") || selectedSong.Language.Equals("turkceArsiv") || selectedSong.Language.Equals("cocuk") || selectedSong.Language.Equals("rusca"))
				{
					playerProcess = System.Diagnostics.Process.Start(@"C:\Program Files\VideoLAN\VLC\VLC.exe", selectedSongFilePath);
				}
				else
				{
					playerProcess = System.Diagnostics.Process.Start(@"C:\Program Files (x86)\Karaoke Builder Player\kbplayer.exe", selectedSongFilePath);
				}
				
			}
			catch (System.ComponentModel.Win32Exception)
			{
				if (selectedSong.Language.Equals("turkce") || selectedSong.Language.Equals("turkceArsiv") || selectedSong.Language.Equals("cocuk") || selectedSong.Language.Equals("rusca"))
				{
					MessageBox.Show("VLC Medya Oynatıcısı yüklü değil. Lütfen VLC Medya Oynatıcısını " + @"'C:\Program Files\'" + " dizinine yükleyin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
				}
				else
				{
					MessageBox.Show("Karaoke Builder Player yüklü değil. Lütfen Karaoke Builder Player uygulamasını " + @"'C:\Program Files (x86)\'" + " dizinine yükleyin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
				}

				return;
			}

			if (activeProcess != null && !activeProcess.HasExited)
			{
				activeProcess.Kill();
			}

			activeProcess = playerProcess;
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
		}

		private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if(Keyboard.IsKeyDown(Key.LeftShift) && Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.D9)) 
			{
				Application.Current.Shutdown();
			}
		}
		
		private void SetUI(int index)
		{
			if (currentIndex != index)
			{
				_categoryText.Text = Song.definitions[index];
				_searchTextBox.Text = "";
				currentSongList = GetMusicList(index);
				currentSongList.Sort();
				_listView.ItemsSource = currentSongList;
				currentIndex = index;
			}
		}

		private void ButtonClicked(object sender, RoutedEventArgs e)
		{
			var tag = Convert.ToInt32((sender as Button).Tag.ToString());
			SetUI(tag);

			_comboBox.SelectedIndex = 0;
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var combobox = sender as ComboBox;
			var tag = combobox.SelectedIndex;

			if(tag != 0)
			{
				SetUI(tag + 1);
			}

		}

		private void ComboBox_Loaded(object sender, RoutedEventArgs e)
		{
			var combobox = sender as ComboBox;

			var list = new List<string>()
			{
				"Seçiniz"
			};

			list.AddRange(Song.definitions);
			list.RemoveAt(1);
			list.RemoveAt(1);

			combobox.ItemsSource = list;
			combobox.SelectedIndex = 0;
		}
	}
}