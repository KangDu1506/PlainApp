using Microsoft.Win32;
using PlainApp.Services;
using PlainApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PlainApp.Partials.PlanEditorUtils
{
    /// <summary>
    /// Interaction logic for TaskEditor.xaml
    /// </summary>
    public partial class TaskEditor : UserControl
    {
        public TaskEditor()
        {
            InitializeComponent();
        }

        private void AddAttachments_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not TaskModel task)
            {
                MessageBox.Show("No task is currently bound to this editor.", "Add attachments", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var choice = MessageBox.Show(
                "Add files?\n\nYes = files\nNo = folder\nCancel = stop",
                "Add attachments",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);

            if (choice == MessageBoxResult.Yes)
            {
                var dialog = new OpenFileDialog
                {
                    Title = "Select attachment file(s)",
                    Filter = "All files (*.*)|*.*",
                    Multiselect = true
                };

                if (dialog.ShowDialog() != true)
                    return;

                AddPaths(task, dialog.FileNames);
                return;
            }

            if (choice == MessageBoxResult.No)
            {
                using var dialog = new System.Windows.Forms.FolderBrowserDialog
                {
                    Description = "Select a folder to attach",
                    UseDescriptionForTitle = true,
                    ShowNewFolderButton = false
                };

                if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK || string.IsNullOrWhiteSpace(dialog.SelectedPath))
                    return;

                AddPaths(task, new[] { dialog.SelectedPath });
            }
        }

        private void AddAttachments_Drop(object sender, DragEventArgs e)
        {
            if (DataContext is not TaskModel task)
                return;

            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            if (e.Data.GetData(DataFormats.FileDrop) is not string[] droppedPaths || droppedPaths.Length == 0)
                return;

            AddPaths(task, droppedPaths);
            e.Handled = true;
        }

        private void AddAttachments_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
            e.Handled = true;
        }

        private async void AddAttachments_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.V || Keyboard.Modifiers != ModifierKeys.Control)
                return;

            if (DataContext is not TaskModel task)
                return;

            if (!Clipboard.ContainsText())
                return;

            string clipboardText = Clipboard.GetText().Trim();
            if (!Uri.TryCreate(clipboardText, UriKind.Absolute, out Uri? uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                return;
            }

            if (task.ExternalAttachments.Any(x =>
                    string.Equals(x.RelativePath, uri.AbsoluteUri, StringComparison.OrdinalIgnoreCase)))
            {
                e.Handled = true;
                return;
            }

            string title = await ExternalSourceUtils.GetURLTitleAsync(uri.AbsoluteUri);
            task.ExternalAttachments.Add(new URLModel
            {
                Name = string.IsNullOrWhiteSpace(title) ? uri.Host : title,
                RelativePath = uri.AbsoluteUri
            });

            e.Handled = true;
        }

        private void AddAttachments_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void AddPaths(TaskModel task, IEnumerable<string> paths)
        {
            foreach (string path in paths)
            {
                AddPath(task, path);
            }
        }

        private void AddPath(TaskModel task, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;

            if (File.Exists(path))
            {
                AddFile(task, path);
                return;
            }

            if (Directory.Exists(path))
            {
                AddFolder(task, path);
            }
        }

        private void AddFile(TaskModel task, string filePath)
        {
            if (task.ExternalAttachments.Any(x =>
                    string.Equals(x.RelativePath, filePath, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            var fileInfo = new FileInfo(filePath);
            task.ExternalAttachments.Add(new FileModel
            {
                Name = Path.GetFileName(filePath),
                RelativePath = filePath,
                ByteSize = fileInfo.Length,
                Extension = fileInfo.Extension
            });
        }

        private void AddFolder(TaskModel task, string folderPath)
        {
            if (task.ExternalAttachments.Any(x =>
                    string.Equals(x.RelativePath, folderPath, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            task.ExternalAttachments.Add(CreateFolderModel(folderPath));
        }

        private FolderModel CreateFolderModel(string folderPath)
        {
            var folder = new FolderModel
            {
                Name = GetDisplayName(folderPath),
                RelativePath = folderPath
            };

            long totalSize = 0;
            foreach (string filePath in Directory.EnumerateFiles(folderPath))
            {
                var fileInfo = new FileInfo(filePath);
                folder.Children.Add(new FileModel
                {
                    Name = Path.GetFileName(filePath),
                    RelativePath = filePath,
                    ByteSize = fileInfo.Length,
                    Extension = fileInfo.Extension
                });
                totalSize += fileInfo.Length;
            }

            foreach (string childFolderPath in Directory.EnumerateDirectories(folderPath))
            {
                var childFolder = CreateFolderModel(childFolderPath);
                folder.Children.Add(childFolder);
                totalSize += childFolder.ByteSize;
            }

            folder.ByteSize = totalSize;
            return folder;
        }

        private static string GetDisplayName(string path)
        {
            string name = Path.GetFileName(path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
            return string.IsNullOrWhiteSpace(name) ? path : name;
        }
    }
}
