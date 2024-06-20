// ファイルのバイナリ版（BOMあり）
function downloadFromByteArray(data) {
    // Blob 型のオブジェクトで包む
    const blob = new Blob([data.byteArray], { type: data.contentType });
    // この Blob オブジェクトにリンクした "オブジェクト URL" を生成
    const url = URL.createObjectURL(blob);
    // 先の節で実装した、指定された URL からのダウンロードを開始する
    // JavaScript 関数を呼び出して、ダウンロードを開始    downloadFromUrl({ url: url, fileName: options.fileName });
    downloadFromUrl(url, data.fileName);
    // 最後に不要リソースの後始末
    URL.revokeObjectURL(url);
}

//ファイルダウンロード
function downloadFromUrl(url, fileName) {
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
}
