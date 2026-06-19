// FUNews Management System - Site JavaScript

// ===== TOAST NOTIFICATIONS =====
function showToast(message, type = 'success') {
    const toastEl = document.getElementById('liveToast');
    const toastMsg = document.getElementById('toastMessage');
    if (!toastEl || !toastMsg) return;

    // Remove old type classes
    toastEl.classList.remove('toast-success', 'toast-danger', 'toast-warning');
    if (type === 'success') toastEl.classList.add('toast-success');
    else if (type === 'danger') toastEl.classList.add('toast-danger');

    toastMsg.textContent = message;
    const toast = bootstrap.Toast.getOrCreateInstance(toastEl, { delay: 3500 });
    toast.show();
}

// ===== AJAX FORM SUBMIT =====
function submitForm(formId, url, onSuccess, onError) {
    const form = document.getElementById(formId);
    if (!form) return;

    const formData = new FormData(form);
    const token = document.querySelector('input[name="__RequestVerificationToken"]');

    fetch(url, {
        method: 'POST',
        headers: { 'RequestVerificationToken': token ? token.value : '' },
        body: formData
    })
    .then(r => r.json())
    .then(data => {
        if (data.success) {
            showToast(data.message, 'success');
            if (onSuccess) onSuccess(data);
        } else {
            showToast(data.message || 'Operation failed.', 'danger');
            if (onError) onError(data);
        }
    })
    .catch(err => {
        showToast('An error occurred. Please try again.', 'danger');
        console.error(err);
    });
}

// ===== CONFIRM DELETE =====
let pendingDeleteUrl = '';
let pendingDeleteCallback = null;

function confirmDelete(message, url, onConfirm) {
    pendingDeleteUrl = url;
    pendingDeleteCallback = onConfirm;

    const msgEl = document.getElementById('deleteConfirmMessage');
    if (msgEl) msgEl.textContent = message || 'Are you sure you want to delete this item?';

    const modal = bootstrap.Modal.getOrCreateInstance(document.getElementById('confirmDeleteModal'));
    modal.show();
}

document.addEventListener('DOMContentLoaded', function () {
    const confirmBtn = document.getElementById('confirmDeleteBtn');
    if (confirmBtn) {
        confirmBtn.addEventListener('click', function () {
            if (!pendingDeleteUrl) return;

            const token = document.querySelector('input[name="__RequestVerificationToken"]');
            const formData = new FormData();
            if (token) formData.append('__RequestVerificationToken', token.value);

            // Extract id from URL params or data
            const urlObj = new URL(pendingDeleteUrl, window.location.origin);
            urlObj.searchParams.forEach((val, key) => formData.append(key, val));

            // Use the url's path for POST
            fetch(urlObj.pathname + urlObj.search, {
                method: 'POST',
                headers: { 'RequestVerificationToken': token ? token.value : '' },
                body: new URLSearchParams(formData)
            })
            .then(r => r.json())
            .then(data => {
                const modal = bootstrap.Modal.getInstance(document.getElementById('confirmDeleteModal'));
                if (modal) modal.hide();

                if (data.success) {
                    showToast(data.message, 'success');
                    if (pendingDeleteCallback) pendingDeleteCallback(data);
                } else {
                    showToast(data.message || 'Delete failed.', 'danger');
                }
                pendingDeleteUrl = '';
                pendingDeleteCallback = null;
            })
            .catch(err => {
                showToast('An error occurred.', 'danger');
                console.error(err);
            });
        });
    }
});

// ===== GLOBAL DELETE HANDLER =====
function deleteItem(url, idField, idValue, onSuccess) {
    const token = document.querySelector('input[name="__RequestVerificationToken"]');
    const body = new URLSearchParams();
    body.append(idField, idValue);
    if (token) body.append('__RequestVerificationToken', token.value);

    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
            'RequestVerificationToken': token ? token.value : ''
        },
        body: body.toString()
    })
    .then(r => r.json())
    .then(data => {
        if (data.success) {
            showToast(data.message, 'success');
            if (onSuccess) onSuccess();
        } else {
            showToast(data.message || 'Delete failed.', 'danger');
        }
    })
    .catch(() => showToast('An error occurred.', 'danger'));
}

// ===== AUTO DISMISS ALERTS =====
document.addEventListener('DOMContentLoaded', function () {
    setTimeout(function () {
        document.querySelectorAll('.alert.alert-success, .alert.alert-info').forEach(el => {
            const alert = bootstrap.Alert.getOrCreateInstance(el);
            alert.close();
        });
    }, 4000);
});
