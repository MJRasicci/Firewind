export async function copyText(text) {
  if (!navigator?.clipboard?.writeText) {
    throw new Error("Clipboard API is not available in this browser context.");
  }

  const value = typeof text === "string" ? text : "";
  await navigator.clipboard.writeText(value);
}
