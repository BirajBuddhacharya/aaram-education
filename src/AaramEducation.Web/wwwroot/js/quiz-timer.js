document.addEventListener('DOMContentLoaded', () => {
    const el = document.getElementById('quiz-timer');
    if (!el) return;
    let secs = 0;
    const fmt = n => String(n).padStart(2, '0');
    setInterval(() => {
        secs++;
        el.textContent = `${fmt(Math.floor(secs / 60))}:${fmt(secs % 60)}`;
    }, 1000);
});
