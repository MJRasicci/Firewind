const defaultStorageKey = "firewind-theme";

function getStorageKey(storageKey) {
  if (typeof storageKey !== "string") {
    return defaultStorageKey;
  }

  const normalizedStorageKey = storageKey.trim();
  return normalizedStorageKey.length > 0 ? normalizedStorageKey : defaultStorageKey;
}

function applyTheme(theme) {
  if (typeof theme !== "string" || theme.trim().length === 0) {
    return;
  }

  document.documentElement.setAttribute("data-theme", theme);
}

export function setTheme(storageKey, theme) {
  if (typeof theme !== "string" || theme.trim().length === 0) {
    return;
  }

  applyTheme(theme);

  try {
    localStorage.setItem(getStorageKey(storageKey), theme);
  } catch {
    // Ignore storage failures (private mode, denied access, etc.).
  }
}

export function loadTheme(storageKey) {
  try {
    const persistedTheme = localStorage.getItem(getStorageKey(storageKey));

    if (typeof persistedTheme !== "string" || persistedTheme.trim().length === 0) {
      return null;
    }

    applyTheme(persistedTheme);
    return persistedTheme;
  } catch {
    return null;
  }
}
