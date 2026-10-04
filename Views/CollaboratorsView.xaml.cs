using PlainApp.Enums;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PlainApp.Views
{
    public partial class CollaboratorsView : UserControl
    {
        public CollaboratorsView()
        {
            InitializeComponent();
        }

        private void OpenChatDetail_Click(object? sender, RoutedEventArgs e)
        {
            SwitchRightView(CollaboratorRightViewType.ChatDetail);
        }

        private void BackToChatList_Click(object? sender, RoutedEventArgs e)
        {
            SwitchRightView(CollaboratorRightViewType.ChatList);
        }

        private void OpenChatInfo_Click(object? sender, RoutedEventArgs e)
        {
            SwitchRightView(CollaboratorRightViewType.ChatInfo);
        }

        private void BackToChatDetail_Click(object? sender, RoutedEventArgs e)
        {
            SwitchRightView(CollaboratorRightViewType.ChatDetail);
        }

        private void SwitchRightView(CollaboratorRightViewType viewType)
        {
            switch (viewType)
            {
                case CollaboratorRightViewType.ChatList:
                    RightContentArea.ContentTemplate = (DataTemplate)FindResource("ChatListTemplate");
                    break;
                case CollaboratorRightViewType.ChatDetail:
                    RightContentArea.ContentTemplate = (DataTemplate)FindResource("ChatDetailTemplate");
                    break;
                case CollaboratorRightViewType.ChatInfo:
                    RightContentArea.ContentTemplate = (DataTemplate)FindResource("ChatInfoTemplate");
                    break;
            }
        }
    }
}
