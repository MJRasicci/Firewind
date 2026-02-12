export function showDialog(elementId) {
  const element = document.getElementById(elementId);
  element.showModal();
}

export function closeDialog(elementId) {
  const element = document.getElementById(elementId);
  element.close();
}
