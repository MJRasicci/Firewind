export function showDialog(elementId) {
  const element = document.getElementById(elementId);
  if (!element || typeof element.showModal !== "function" || element.open) {
    return;
  }

  element.showModal();
}

export function closeDialog(elementId) {
  const element = document.getElementById(elementId);
  if (!element || typeof element.close !== "function" || !element.open) {
    return;
  }

  element.close();
}
