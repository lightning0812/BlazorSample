// ファイルダウンロード
function downloadFile(data) {
    // Blob作成
    const blob = new Blob([data.byteArray], { type: data.contentType });

    // ダウンロード可能なリンクを作成
    const url = URL.createObjectURL(blob);

    // URL からのダウンロードを開始する
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = data.fileName ?? '';
    anchorElement.click();
    anchorElement.remove();

    // URLの後始末
    URL.revokeObjectURL(url);
}
