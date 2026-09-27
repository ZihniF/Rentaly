const path = require("path");
const fs = require("fs");
const { chromium } = require("C:/Users/Zgf/.cache/codex-runtimes/codex-primary-runtime/dependencies/node/node_modules/playwright");

const baseUrl = "http://127.0.0.1:5158";
const outputDirectory = path.resolve(__dirname, "..", "docs", "screenshots");
const chromiumPath = "C:/Program Files/Google/Chrome/Application/chrome.exe";

fs.mkdirSync(outputDirectory, { recursive: true });

async function preparePage(page) {
    await page.waitForLoadState("networkidle");
    await page.evaluate(async () => {
        if (document.fonts?.ready) await document.fonts.ready;
    });
    await page.addStyleTag({
        content: `
            *, *::before, *::after {
                animation-duration: 0s !important;
                animation-delay: 0s !important;
                transition: none !important;
                caret-color: transparent !important;
            }
            html { scroll-behavior: auto !important; }
        `
    });
}

async function capture(page, fileName, route, options = {}) {
    await page.goto(`${baseUrl}${route}`, { waitUntil: "domcontentloaded" });
    await preparePage(page);

    if (options.scrollTo) {
        await page.locator(options.scrollTo).first().scrollIntoViewIfNeeded();
        await page.waitForTimeout(150);
    } else {
        await page.evaluate(() => window.scrollTo(0, 0));
    }

    const filePath = path.join(outputDirectory, fileName);
    await page.screenshot({
        path: filePath,
        fullPage: options.fullPage ?? false,
        animations: "disabled"
    });
    console.log(`${fileName} <- ${route}`);
}

(async () => {
    const browser = await chromium.launch({
        headless: true,
        executablePath: chromiumPath,
        args: ["--disable-gpu", "--hide-scrollbars"]
    });

    try {
        const context = await browser.newContext({
            viewport: { width: 1600, height: 1000 },
            deviceScaleFactor: 1,
            locale: "tr-TR",
            colorScheme: "light"
        });
        const page = await context.newPage();

        await capture(page, "home-page.png", "/");

        const bookingHref = await page.locator('a[href*="/Booking"]').first().getAttribute("href");
        if (!bookingHref) throw new Error("Ana sayfada rezervasyon bağlantısı bulunamadı.");
        const bookingUrl = new URL(bookingHref, baseUrl);
        const carId = bookingUrl.searchParams.get("carId");
        if (!carId) throw new Error("Rezervasyon bağlantısından araç kimliği alınamadı.");

        await capture(page, "fleet.png", "/Fleet");
        await capture(page, "booking.png", `/Booking?carId=${encodeURIComponent(carId)}`);
        await capture(page, "car-detail.png", `/Car/Detail/${encodeURIComponent(carId)}`);
        await capture(page, "admin-dashboard.png", "/admin");
        await capture(page, "admin-rentals.png", "/Admin/Rentals");
        await capture(page, "admin-cars.png", "/Car/CarList");
        await capture(page, "admin-brands-models.png", "/Brand/BrandList");
        await capture(page, "admin-home-contents.png", "/Admin/HomeContents");

        await page.setContent(`
            <!doctype html>
            <html lang="tr">
            <head>
                <meta charset="utf-8">
                <style>
                    * { box-sizing: border-box; }
                    body { margin: 0; padding: 58px; background: #eef2f6; color: #1f2937; font-family: Arial, sans-serif; }
                    .mail { width: 720px; margin: 0 auto; background: #fff; border-radius: 20px; overflow: hidden; box-shadow: 0 18px 55px rgba(15,23,42,.14); }
                    .head { padding: 36px 42px; color: #fff; background: #0d1117; }
                    .head small { display: block; color: #f4b942; font-weight: 700; letter-spacing: 2px; margin-bottom: 10px; }
                    .head h1 { margin: 0; font-size: 34px; }
                    .body { padding: 38px 42px 34px; font-size: 17px; line-height: 1.6; }
                    .summary { width: 100%; margin: 24px 0; padding: 20px; background: #f8fafc; border-radius: 14px; border-collapse: separate; border-spacing: 0 10px; }
                    .summary td:last-child { text-align: right; }
                    .coupon { margin: 24px 0; padding: 23px; border-radius: 16px; color: #fff; background: linear-gradient(125deg,#da9b20,#f2c45b); position: relative; overflow: hidden; }
                    .coupon::after { content: "RENTALY"; position: absolute; right: -8px; bottom: -22px; font-size: 58px; font-weight: 900; opacity: .16; }
                    .coupon span { display: block; font-size: 13px; letter-spacing: 2px; }
                    .coupon strong { display: block; margin-top: 4px; font-size: 25px; }
                    .signature { margin-top: 28px; color: #475569; }
                    .signature strong { color: #111827; }
                </style>
            </head>
            <body>
                <div class="mail">
                    <div class="head"><small>RENTALY REZERVASYON</small><h1>Yola çıkmaya hazırsınız.</h1></div>
                    <div class="body">
                        <p>Merhaba Değerli Misafirimiz,</p>
                        <p><strong>Renault Clio</strong> rezervasyonunuz onaylandı.</p>
                        <table class="summary">
                            <tr><td>Alış</td><td><strong>10.10.2026 10:00</strong></td></tr>
                            <tr><td>İade</td><td><strong>13.10.2026 10:00</strong></td></tr>
                            <tr><td>Toplam</td><td><strong>4.500,00 ₺</strong></td></tr>
                        </table>
                        <div class="coupon"><span>SIZE ÖZEL</span><strong>Bir sonraki rezervasyonunuz için indirim</strong></div>
                        <p class="signature">İyi yolculuklar,<br><strong>Rentaly Rezervasyon Ekibi</strong><br>Güvenli yolculuğunuz için yanınızdayız.</p>
                    </div>
                </div>
            </body>
            </html>
        `, { waitUntil: "load" });
        await page.screenshot({ path: path.join(outputDirectory, "approval-email.png"), fullPage: false });
        console.log("approval-email.png <- e-posta şablonu önizlemesi");

        await context.close();
    } finally {
        await browser.close();
    }
})().catch(error => {
    console.error(error);
    process.exitCode = 1;
});
