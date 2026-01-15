import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { AppSettings, DEFAULT_SETTINGS } from '../models/settings';

const STORAGE_KEY = 'spoolman-updater-settings';

@Injectable({
  providedIn: 'root'
})
export class SettingsService {
  private settingsSubject: BehaviorSubject<AppSettings>;
  settings$: Observable<AppSettings>;

  constructor() {
    const storedSettings = this.loadFromStorage();
    this.settingsSubject = new BehaviorSubject<AppSettings>(storedSettings);
    this.settings$ = this.settingsSubject.asObservable();

    // Apply settings on initialization
    this.applySettings(storedSettings);
  }

  get settings(): AppSettings {
    return this.settingsSubject.value;
  }

  updateSettings(settings: Partial<AppSettings>): void {
    const newSettings = { ...this.settingsSubject.value, ...settings };
    this.settingsSubject.next(newSettings);
    this.saveToStorage(newSettings);
    this.applySettings(newSettings);
  }

  resetToDefaults(): void {
    this.settingsSubject.next(DEFAULT_SETTINGS);
    this.saveToStorage(DEFAULT_SETTINGS);
    this.applySettings(DEFAULT_SETTINGS);
  }

  private loadFromStorage(): AppSettings {
    try {
      const stored = localStorage.getItem(STORAGE_KEY);
      if (stored) {
        const parsed = JSON.parse(stored);
        // Merge with defaults to handle new settings added in updates
        return { ...DEFAULT_SETTINGS, ...parsed };
      }
    } catch (e) {
      console.error('Failed to load settings from storage:', e);
    }
    return DEFAULT_SETTINGS;
  }

  private saveToStorage(settings: AppSettings): void {
    try {
      localStorage.setItem(STORAGE_KEY, JSON.stringify(settings));
    } catch (e) {
      console.error('Failed to save settings to storage:', e);
    }
  }

  private applySettings(settings: AppSettings): void {
    // Apply accent color as CSS custom property
    document.documentElement.style.setProperty('--accent-color', settings.accentColor);

    // Calculate darker and lighter variants for hover states etc.
    const hsl = this.hexToHSL(settings.accentColor);
    if (hsl) {
      const darkerL = Math.max(0, hsl.l - 10);
      const lighterL = Math.min(100, hsl.l + 20);
      document.documentElement.style.setProperty(
        '--accent-color-dark',
        `hsl(${hsl.h}, ${hsl.s}%, ${darkerL}%)`
      );
      document.documentElement.style.setProperty(
        '--accent-color-light',
        `hsl(${hsl.h}, ${hsl.s}%, ${lighterL}%)`
      );
    }
  }

  private hexToHSL(hex: string): { h: number; s: number; l: number } | null {
    // Remove # if present
    hex = hex.replace('#', '');

    if (hex.length !== 6) return null;

    const r = parseInt(hex.substring(0, 2), 16) / 255;
    const g = parseInt(hex.substring(2, 4), 16) / 255;
    const b = parseInt(hex.substring(4, 6), 16) / 255;

    const max = Math.max(r, g, b);
    const min = Math.min(r, g, b);
    let h = 0;
    let s = 0;
    const l = (max + min) / 2;

    if (max !== min) {
      const d = max - min;
      s = l > 0.5 ? d / (2 - max - min) : d / (max + min);

      switch (max) {
        case r:
          h = ((g - b) / d + (g < b ? 6 : 0)) / 6;
          break;
        case g:
          h = ((b - r) / d + 2) / 6;
          break;
        case b:
          h = ((r - g) / d + 4) / 6;
          break;
      }
    }

    return {
      h: Math.round(h * 360),
      s: Math.round(s * 100),
      l: Math.round(l * 100),
    };
  }
}
