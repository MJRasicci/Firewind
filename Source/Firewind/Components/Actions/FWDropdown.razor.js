const outsideClickHandlers = new Map();

function getDropdownElement(elementId) {
  if (typeof elementId !== "string" || elementId.trim().length === 0) {
    return null;
  }

  const element = document.getElementById(elementId);
  return element instanceof HTMLDetailsElement ? element : null;
}

export function configureOutsideClickClose(elementId, closeOnOutsideClick) {
  removeOutsideClickClose(elementId);

  if (closeOnOutsideClick !== true) {
    return;
  }

  const element = getDropdownElement(elementId);
  if (!element) {
    return;
  }

  const handler = (event) => {
    if (!element.open) {
      return;
    }

    const target = event.target;
    if (target instanceof Node && element.contains(target)) {
      return;
    }

    element.open = false;
  };

  document.addEventListener("pointerdown", handler, true);
  outsideClickHandlers.set(elementId, handler);
}

export function removeOutsideClickClose(elementId) {
  const handler = outsideClickHandlers.get(elementId);
  if (!handler) {
    return;
  }

  document.removeEventListener("pointerdown", handler, true);
  outsideClickHandlers.delete(elementId);
}
