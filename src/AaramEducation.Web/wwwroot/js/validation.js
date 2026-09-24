// Password strength meter
const pwInput = document.getElementById('Password');
const pwStrengthEl = document.getElementById('pw-strength');

if (pwInput && pwStrengthEl) {
  pwInput.addEventListener('input', () => {
    const v = pwInput.value;
    let score = 0;
    if (v.length >= 8) score++;
    if (/[A-Z]/.test(v)) score++;
    if (/[0-9]/.test(v)) score++;
    if (/[^A-Za-z0-9]/.test(v)) score++;

    const levels = [
      { label: 'Too short', color: '#dc3545' },
      { label: 'Weak',      color: '#fd7e14' },
      { label: 'Fair',      color: '#ffc107' },
      { label: 'Good',      color: '#4A7C59' },
      { label: 'Strong',    color: '#198754' },
    ];
    const { label, color } = levels[score] ?? levels[4];

    pwStrengthEl.innerHTML = `
      <div class="pw-strength-bar" style="width:${score * 25}%;background:${color}"></div>
      <div class="pw-strength-label" style="color:${color}">${label}</div>`;
  });
}

// Role card radio toggling
document.querySelectorAll('.role-card input[type="radio"]').forEach(radio => {
  radio.addEventListener('change', () => {
    document.querySelectorAll('.role-card').forEach(c => c.classList.remove('selected'));
    if (radio.checked) radio.closest('.role-card').classList.add('selected');
  });
  if (radio.checked) radio.closest('.role-card').classList.add('selected');
});

// Required field inline feedback on blur
document.querySelectorAll('input[required], select[required]').forEach(el => {
  el.addEventListener('blur', () => {
    if (!el.value.trim()) {
      el.classList.add('is-invalid');
    } else {
      el.classList.remove('is-invalid');
      el.classList.add('is-valid');
    }
  });
});
