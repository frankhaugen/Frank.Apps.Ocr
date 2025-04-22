window.pdfViewer = {
    pdfDoc: null,
    currentPage: 1,
    totalPages: 0,
    scale: 1.0,
    canvas: null,
    ctx: null,

    loadPdf: async function (base64Data, canvasId) {
        const loadingTask = pdfjsLib.getDocument({ data: atob(base64Data) });
        this.pdfDoc = await loadingTask.promise;
        this.totalPages = this.pdfDoc.numPages;
        this.canvas = document.getElementById(canvasId);
        this.ctx = this.canvas.getContext('2d');
        await this.renderPage(this.currentPage);
        return this.totalPages;
    },

    renderPage: async function (num) {
        const page = await this.pdfDoc.getPage(num);
        const viewport = page.getViewport({ scale: this.scale });
        this.canvas.height = viewport.height;
        this.canvas.width = viewport.width;

        const renderContext = {
            canvasContext: this.ctx,
            viewport: viewport
        };
        await page.render(renderContext).promise;
    },

    nextPage: async function () {
        if (this.currentPage >= this.totalPages) return;
        this.currentPage++;
        await this.renderPage(this.currentPage);
    },

    prevPage: async function () {
        if (this.currentPage <= 1) return;
        this.currentPage--;
        await this.renderPage(this.currentPage);
    },

    getCurrentPage: function () {
        return this.currentPage;
    },
    
    getPageRectangle: async function () {
        const canvas = document.getElementById('pdfCanvas');
        if (!canvas) return null;
        const rect = canvas.getBoundingClientRect();
        return {
            x: rect.left,
            y: rect.top,
            width: rect.width,
            height: rect.height
        };
    },

    getCurrentScale: function () {
        return this.scale;
    },

    drawRectangleOnCanvas: function (rect) {
        if (!this.ctx) return;
        this.ctx.strokeStyle = 'rgba(255, 0, 0, 0.8)';
        this.ctx.lineWidth = 2;
        this.ctx.strokeRect(rect.x, rect.y, rect.width, rect.height);
    },
    
    drawRectangleOnCanvasWithScale: function (rect) {
        if (!this.ctx) return;
        // Get the canvas element as a canvas
        const canvas = document.getElementsByClassName('blazorpdf-drawing__canvas')[0];
        if (!canvas) return;
        
        // Calculate the scaled coordinates
        const scale = this.getCurrentScale();
        const canvasRect = canvas.getBoundingClientRect();
        const x = (rect.x - canvasRect.left) / scale;
        const y = (rect.y - canvasRect.top) / scale;
        const width = rect.width / scale;
        const height = rect.height / scale;
        this.ctx.strokeStyle = 'rgba(255, 0, 0, 0.8)';
        this.ctx.lineWidth = 2;
        this.ctx.strokeRect(x, y, width, height);
    }
};
