Thêm AppFlyers:<br>
B1: Thêm Unitypackage vào project<br>
B2: Kéo AppFlyer.prefab (Ở GplayAppFlyer/Prefabs) vào scene Loading ở GameController<br>
B3: Vào `Assets/External Dependency Manager/Android Resolver/Force Resolve` để resolve các dependencies<br>

Thêm AppFlyers Revenue Tracking:<br>
B1: Vào class AnalyticsController.cs thêm dòng `[Obsolete("",true)]` trước method `public static void LogAdsRevenue`<br>
B2: Đi theo các lỗi đang hiện rồi chạy method `AppFlyerGplay.LogRevenue()`.<br>
Lưu ý:
- Có thể có nhiều `AnalyticsController.LogAdsRevenue` tuy nhiên chỉ log `AppFlyerGplay.LogRevenue()` một lần ở trước tất cả.
- Dựa vào `AnalyticsController.LogAdsRevenue` đang có để điền thêm các thông tin cần thiết cho `AppFlyerGplay.LogRevenue()` (Xem hd1.png).<br>
- Nếu dùng VSS keymap có thể bấm `Ctrl+Shift+Page Down` để tới lỗi tiếp theo cho nhanh.

B3: Xóa `[Obsolete("",true)]` ở method `AnalyticsController.LogAdsRevenue`<br>
B4: Vào script `AdmobAds.cs` kiếm hàm `LoadBannerAdmob` rồi bên trong hàm đó có hàm `OnAdPaid` thì thêm dòng `AppFlyerGplay.LogRevenue("banner", "admob", "banner", "banner", revenue, adValue.CurrencyCode);` ở đầu method Paid đó (Có thể không kiếm đự hàm LoadBannerAdmob nếu không có thì skip)<br> 