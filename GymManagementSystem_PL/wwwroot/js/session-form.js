document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form[data-prefill-dates], form");
    const startInput = document.querySelector('input[name="StartDate"]');
    const endInput = document.querySelector('input[name="EndDate"]');
    if (!form || !startInput || !endInput) return;

    // yyyy-MM-ddTHH:mm in the browser's local time (toISOString always prints UTC,
    // so shift the timestamp by the zone offset first)
    const toLocalInput = (date) =>
        new Date(date.getTime() - date.getTimezoneOffset() * 60000).toISOString().slice(0, 16);

    const now = new Date();
    startInput.min = toLocalInput(now);
    endInput.min = startInput.value || toLocalInput(now);

    // Only the Create form pre-fills: start = now, end = now + 1 hour
    if (form.hasAttribute("data-prefill-dates")) {
        if (!startInput.value) startInput.value = toLocalInput(now);
        if (!endInput.value) endInput.value = toLocalInput(new Date(now.getTime() + 60 * 60 * 1000));
        endInput.min = startInput.value;
    }

    startInput.addEventListener("change", function () {
        endInput.min = startInput.value;
        if (endInput.value && endInput.value < startInput.value) endInput.value = "";
    });
});