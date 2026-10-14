# TỔNG HỢP CÁC THAY ĐỔI TRONG ỨNG DỤNG PLAINAPP

Tài liệu này tổng hợp toàn bộ các thay đổi kỹ thuật và cải tiến đã thực hiện trên mã nguồn ứng dụng PlainApp.

---

## 1. Tách biệt tầng hiển thị giao diện (Presentation Layer Refactoring)

### Mục tiêu & Nguyên tắc
Tách toàn bộ các thuộc tính định dạng trực quan tĩnh (màu sắc, phông chữ, kích thước, căn lề `Margin`/`Padding`, bo góc viền, hiệu ứng chuột) ra khỏi các file giao diện XAML (`Views/` và `Partials/`), chuyển về quản lý tập trung trong các `ResourceDictionary` tương ứng ở thư mục `Styles/`. 

Các file View giờ đây chỉ giữ vai trò định nghĩa cấu trúc khung nhìn (Layout: `Grid`, hàng/cột) và liên kết dữ liệu (`DataBinding`, `Command`).

### Các thành phần đã được refactor:
- **`Partials/Header.xaml`**:
  - Tách style khối tiêu đề ứng dụng `AppHeaderTextBlock`.
  - Tách style khung viền `HeaderContainerBorderStyle`.
  - Tách các style chọn ngôn ngữ và chủ đề giao diện (`ThemeSelectorComboBoxStyle`, `LanguageSelectorComboBoxStyle`) vào `Styles/Partials/HeaderStyles.xaml`.
- **`Partials/SideMenu.xaml`**:
  - Tách style các nút điều hướng chính/phụ (`SideMenuPrimaryButtonStyle`, `SideMenuButtonStyle`, `SideMenuBottomHelpButtonStyle`).
  - Tách style lớp phủ mờ `SideMenuOverlayBorderStyle` và container `SideMenuContainerBorderStyle` vào `Styles/Partials/SideMenuStyles.xaml`.
- **`Partials/PlanEditorUtils/TaskEditor.xaml`**:
  - Tách style ô nhập tiêu đề task `TitleTextBox` (font size 24, font weight black, trigger gạch chân khi focus).
  - Tách style ô mô tả `DescriptionTextBox`, các nhãn thuộc tính `GeneralAttributeLabel`, ô ngày tháng `DateAttributeTextBox` vào `Styles/Partials/PlanEditorStyles/TaskEditorStyles.xaml`.
- **`Views/HomeView.xaml`**:
  - Chuẩn hóa style cho khung tác vụ sắp tới (`UpcomingTasksContainerStyle`, `UpcomingTasksHeaderStyle`).
  - Chuẩn hóa style lịch tháng (`CalendarContainerStyle`, các nút chuyển tháng/năm, tiêu đề thứ trong tuần `DayOfWeekStyle`) vào `Styles/Views/HomeViewStyles.xaml`.
- **`Views/MyPlansView.xaml`**:
  - Tách style toàn bộ bảng kế hoạch: `DataGridColumnHeaderStyle`, `DataGridRowStyle`, `DataGridCellStyle`, `PlansDataGrid`, thanh tiến độ `ProgressBarStyle`.
  - Tách style thanh tìm kiếm và menu ẩn/hiện cột vào `Styles/Views/MyPlansViewStyles.xaml`.
- **`Views/CollaboratorsView.xaml`**:
  - Tách style khung container, nhãn tìm kiếm `SearchLabel`, ô nhập tìm kiếm `SearchTextBox`.
  - Tách style cho các thành phần bên trong các DataTemplate của chat (`ChatHeaderTitleStyle`, `ChatListItemButtonStyle`, `ChatNavButtonStyle`, `ChatDetailContentBorderStyle`, `ChatInfoContentBorderStyle`, `ChatInfoMemberTextStyle`, `ChatInfoDetailTextStyle`, `ChatInfoRoleTextStyle`) vào `Styles/Views/CollaboratorsViewStyles.xaml`.

---

## 2. Chuẩn hóa và gom Style dùng chung (Styles Consolidation)

### Mục tiêu
Loại bỏ mã nguồn trùng lặp giữa các file Style ResourceDictionary, đưa các style dùng chung vào `Styles/SharedStyles.xaml` và tái sử dụng qua `ResourceDictionary.MergedDictionaries`.

### Các Style đã được chuẩn hóa vào `SharedStyles.xaml`:
1. **`SelectorComboBoxStyle`**:
   - Template ComboBox tùy biến cao (~110 dòng XAML bao gồm `ControlTemplate`, `ToggleButton`, `Popup` bo góc 10px, style cho `ComboBoxItem` và các trigger đổi màu khi chuột rê/chọn).
   - Trước đây bị sao chép nguyên mẫu ở cả `SharedStyles.xaml` và `HeaderStyles.xaml`. Đã xóa bỏ toàn bộ mã lặp ở `HeaderStyles.xaml` và dùng trực tiếp từ `SharedStyles.xaml`.
2. **`SearchButtonStyle`**:
   - Style nút tìm kiếm hình tròn kích thước 30x30px, bo góc 25px, hỗ trợ trigger hover (`70%TransparentBlackBrush`) và pressed (`DeepDarkBlueBrush`).
   - Trước đây bị định nghĩa trùng lặp 100% giữa `CollaboratorsViewStyles.xaml` và `MyPlansViewStyles.xaml`. Đã gom về `SharedStyles.xaml` và xóa bỏ mã lặp tại cả hai view styles.
3. **`MainContainerBorderStyle`**:
   - Khung viền container nền đen trong suốt (`70%TransparentBlackBrush`), bo góc 16px được chuẩn hóa để các view sử dụng thống nhất.

*Kết quả:* Giảm thiểu hơn 135 dòng mã XAML dư thừa, giúp việc thay đổi màu sắc/chủ đề toàn ứng dụng chỉ cần chỉnh sửa tại một nơi duy nhất.

---

## 3. Sửa lỗi Runtime Crash trong màn hình Chat (CollaboratorsView)

### Hiện tượng lỗi
Khi chuyển sang màn hình Collaborators (`Ctrl+4`) và click vào một đoạn chat trong danh sách (`💬 Chat với Nguyễn Văn A`), ứng dụng bị crash ngay lập tức với ngoại lệ:
`System.Windows.Markup.XamlParseException: Cannot find resource named 'ChatDetailTitleStyle'`.

### Nguyên nhân
- Khi click mở đoạn chat, sự kiện `OpenChatDetail_Click` kích hoạt và chuyển `RightContentArea.ContentTemplate` sang `ChatDetailTemplate`.
- Cả `ChatDetailTemplate` và `ChatInfoTemplate` đều sử dụng:
  ```xml
  <TextBlock Grid.Column="1" Text="..." Style="{StaticResource ChatDetailTitleStyle}"/>
  ```
- Style `ChatDetailTitleStyle` bị thiếu trong `Styles/Views/CollaboratorsViewStyles.xaml`. Do nằm bên trong `DataTemplate` (cơ chế nạp trễ runtime của WPF), lỗi chỉ phát sinh khi người dùng click để nạp giao diện này.

### Cách khắc phục
- Bổ sung định nghĩa `ChatDetailTitleStyle` đầy đủ vào [Styles/Views/CollaboratorsViewStyles.xaml](Styles/Views/CollaboratorsViewStyles.xaml):
  ```xml
  <Style x:Key="ChatDetailTitleStyle" TargetType="TextBlock">
      <Setter Property="FontSize" Value="16"/>
      <Setter Property="FontWeight" Value="Bold"/>
      <Setter Property="FontFamily" Value="{DynamicResource AppCurrentFont}"/>
      <Setter Property="Foreground" Value="{DynamicResource BareWhiteBrush}"/>
      <Setter Property="VerticalAlignment" Value="Center"/>
  </Style>
  ```
- Chuẩn hóa đường dẫn tài nguyên `MergedDictionaries` trong `Views/CollaboratorsView.xaml` sang đường dẫn tương đối `../Styles/Views/CollaboratorsViewStyles.xaml`.
- Sau khi sửa, các thao tác chuyển đổi qua lại giữa danh sách chat, chi tiết cuộc trò chuyện và thông tin người dùng đều hoạt động ổn định và mượt mà.

---

## 4. Bảng tổng hợp các file mã nguồn được chỉnh sửa

| Đường dẫn file | Loại chỉnh sửa | Nội dung thay đổi cụ thể |
| :--- | :--- | :--- |
| `Styles/SharedStyles.xaml` | Bổ sung style chung | Bổ sung `SearchButtonStyle`, `MainContainerBorderStyle`, hoàn thiện `SelectorComboBoxStyle`. |
| `Styles/Partials/HeaderStyles.xaml` | Rút gọn mã nguồn | Merge `SharedStyles.xaml`, loại bỏ ~110 dòng `SelectorComboBoxStyle` trùng lặp. |
| `Styles/Views/CollaboratorsViewStyles.xaml` | Bổ sung & dọn dẹp | Merge `SharedStyles.xaml`, loại bỏ `SearchButtonStyle` trùng lặp, thêm `ChatDetailTitleStyle` còn thiếu. |
| `Styles/Views/MyPlansViewStyles.xaml` | Rút gọn mã nguồn | Loại bỏ `SearchButtonStyle` trùng lặp, dùng từ `SharedStyles.xaml`. |
| `Styles/Views/HomeViewStyles.xaml` | Đồng bộ kế thừa | Kế thừa `SelectorComboBoxStyle` và `RefreshButtonStyle` từ `SharedStyles.xaml`. |
| `Styles/Partials/SideMenuStyles.xaml` | Bổ sung style | Định nghĩa các style nút bấm, container và overlay cho SideMenu. |
| `Styles/Partials/PlanEditorStyles/TaskEditorStyles.xaml` | Bổ sung style | Định nghĩa các style ô nhập liệu và nhãn cho TaskEditor. |
| `Partials/Header.xaml` | Dọn dẹp View | Chuyển toàn bộ inline format sang sử dụng các StaticResource style. |
| `Partials/SideMenu.xaml` | Dọn dẹp View | Chuyển toàn bộ inline format sang sử dụng các StaticResource style. |
| `Partials/PlanEditorUtils/TaskEditor.xaml` | Dọn dẹp View | Chuyển toàn bộ inline format sang sử dụng các StaticResource style. |
| `Views/CollaboratorsView.xaml` | Sửa lỗi & dọn dẹp | Dọn dẹp inline format, chuẩn hóa đường dẫn MergedDictionary. |
| `Views/HomeView.xaml` | Dọn dẹp View | Chuyển toàn bộ inline format sang sử dụng các StaticResource style. |
| `Views/MyPlansView.xaml` | Dọn dẹp View | Chuyển toàn bộ inline format sang sử dụng các StaticResource style. |

---

## 5. Kết quả kiểm thử & Trạng thái ứng dụng

- **Biên dịch dự án:** `dotnet build` hoàn thành với **0 Warning, 0 Error**.
- **Tính toàn vẹn tài nguyên:** Đã quét toàn bộ project, 100% các khóa `StaticResource` đều tồn tại và được định nghĩa hợp lệ.
- **Runtime test:** Đã kiểm tra tự động việc khởi tạo tất cả các View (`HomeView`, `MyPlansView`, `NewView`, `CollaboratorsView`) và nạp các DataTemplate (`ChatListTemplate`, `ChatDetailTemplate`, `ChatInfoTemplate`) mà không phát sinh bất kỳ lỗi nào.
