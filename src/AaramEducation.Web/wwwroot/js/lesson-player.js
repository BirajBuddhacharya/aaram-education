document.addEventListener('DOMContentLoaded', () => {
    const container = document.getElementById('video-container');
    const video = document.getElementById('lesson-video');
    if (!video || !container) return;

    const lessonId = parseInt(container.dataset.lessonId, 10);
    const resume = parseInt(container.dataset.resume, 10) || 0;

    if (resume > 0) {
        video.addEventListener('loadedmetadata', () => { video.currentTime = resume; }, { once: true });
    }

    let lastSaved = 0;
    setInterval(() => {
        if (video.paused || video.ended) return;
        const pos = Math.floor(video.currentTime);
        if (pos === lastSaved) return;
        lastSaved = pos;

        fetch('/Lessons/SaveProgress', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': window.aaramAntiForgeryToken ?? '',
            },
            body: JSON.stringify({ lessonId, positionSeconds: pos }),
        }).catch(() => {});
    }, 10_000);
});
