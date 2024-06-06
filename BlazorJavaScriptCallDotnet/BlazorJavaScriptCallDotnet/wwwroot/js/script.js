async function callDotNetMethod () {
    // 非同期で .NETメソッドを呼び出し、結果を取得する例
    const result = await DotNet.invokeMethodAsync('BlazorJavaScriptCallDotnet', 'GetMessage');
    // 取得結果である「.NET -> JavaScript へのメッセージ」がコンソールへ表示されます
    console.log(result); 

    // 非同期で .NETメソッドにデータ（引数）を渡して呼び出す例
    await DotNet.invokeMethodAsync('BlazorJavaScriptCallDotnet', 'UpdateMessage', 'JavaScript -> .NET へのメッセージ');
};