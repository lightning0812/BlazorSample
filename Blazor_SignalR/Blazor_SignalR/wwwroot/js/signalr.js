
// SignalRの接続を切断（ただし、再接続の設定がされている場合は、切断後に再接続を試みる）
function forceCloseConnection() {
    Blazor._internal.forceCloseConnection();
};

// SignalRの接続を切断（再接続の設定がされていても、再接続されない）
function disconnect() {
    Blazor.disconnect();
}

// Blazorを手動で開始（SignalRを接続する）
// 事前に_Layout.cshtml の以下の箇所を修正する必要あり。（autostart="false" 属性を追加）
// <script src="_framework/blazor.server.js"></script> → <script src="_framework/blazor.server.js" autostart="false"></script>
function startBlazor() {
    // 接続を開始する際にオプションを付与することができる
    Blazor.start({
        reconnectionOptions: {
            maxRetries: 3,                      // SignalRが切れた場合、再接続の最大試行回数
            retryIntervalMilliseconds: 2000,    // どのくらい感覚を空けて再接続を試みるか（単位:ミリ秒）
        }
    });
}