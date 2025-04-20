export async function renderPdfWithOcr(pdfDataBase64, altoData) {
    const pdfData = atob(pdfDataBase64);
    const loadingTask = pdfjsLib.getDocument({ data: pdfData });
    const pdf = await loadingTask.promise;

    const viewer = document.getElementById('pdfViewer');
    viewer.innerHTML = '';

    for (let pageNum = 1; pageNum <= pdf.numPages; pageNum++) {
        const page = await pdf.getPage(pageNum);
        const viewport = page.getViewport({ scale: 1.5 });

        const canvas = document.createElement('canvas');
        canvas.width = viewport.width;
        canvas.height = viewport.height;
        const context = canvas.getContext('2d');

        const renderContext = {
            canvasContext: context,
            viewport: viewport
        };
        await page.render(renderContext).promise;

        const overlay = document.createElement('div');
        overlay.className = 'ocr-overlay';
        overlay.style.position = 'absolute';
        overlay.style.top = '0';
        overlay.style.left = '0';
        overlay.style.width = `${viewport.width}px`;
        overlay.style.height = `${viewport.height}px`;
        overlay.style.pointerEvents = 'none';

        // Assuming altoData.Layout.Page[pageNum - 1] corresponds to the current page
        const altoPage = altoData.Layout.Page[pageNum - 1];
        const printSpace = altoPage.PrintSpace;

        for (const composedBlock of printSpace.ComposedBlock) {
            for (const textBlock of composedBlock.TextBlock) {
                for (const textLine of textBlock.TextLine) {
                    for (const word of textLine.String) {
                        const wordDiv = document.createElement('div');
                        wordDiv.className = 'ocr-word';
                        wordDiv.textContent = word.CONTENT;
                        wordDiv.style.position = 'absolute';
                        wordDiv.style.left = `${parseFloat(word.HPOS) * viewport.scale}px`;
                        wordDiv.style.top = `${parseFloat(word.VPOS) * viewport.scale}px`;
                        wordDiv.style.width = `${parseFloat(word.WIDTH) * viewport.scale}px`;
                        wordDiv.style.height = `${parseFloat(word.HEIGHT) * viewport.scale}px`;
                        wordDiv.style.backgroundColor = 'rgba(255, 255, 0, 0.3)';
                        wordDiv.style.border = '1px solid rgba(255, 255, 0, 0.5)';
                        overlay.appendChild(wordDiv);
                    }
                }
            }
        }

        const pageContainer = document.createElement('div');
        pageContainer.className = 'pdf-page-container';
        pageContainer.style.position = 'relative';
        pageContainer.style.width = `${viewport.width}px`;
        pageContainer.style.height = `${viewport.height}px`;
        pageContainer.appendChild(canvas);
        pageContainer.appendChild(overlay);

        viewer.appendChild(pageContainer);
    }
}
