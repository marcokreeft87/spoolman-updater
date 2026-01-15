export interface AppSettings {
  accentColor: string;
  showSpoolIds: boolean;
}

export const DEFAULT_SETTINGS: AppSettings = {
  accentColor: '#9c27b0', // Default purple color matching Angular Material purple-green theme
  showSpoolIds: false,
};
