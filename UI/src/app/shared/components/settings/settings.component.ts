import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Subject, takeUntil } from 'rxjs';
import { AppSettings } from '../../models/settings';
import { SettingsService } from '../../service/settings.service';

// Predefined color palette for easy selection
const COLOR_PRESETS = [
  { name: 'Purple', hex: '#9c27b0' },
  { name: 'Blue', hex: '#2196f3' },
  { name: 'Teal', hex: '#009688' },
  { name: 'Green', hex: '#4caf50' },
  { name: 'Orange', hex: '#ff9800' },
  { name: 'Red', hex: '#f44336' },
  { name: 'Pink', hex: '#e91e63' },
  { name: 'Indigo', hex: '#3f51b5' },
];

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatCardModule,
    MatCheckboxModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
  ],
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss'],
})
export class SettingsComponent implements OnInit, OnDestroy {
  settings: AppSettings;
  colorPresets = COLOR_PRESETS;
  customColor: string = '';

  private destroy$ = new Subject<void>();

  constructor(
    private settingsService: SettingsService,
    private dialogRef: MatDialogRef<SettingsComponent>
  ) {
    this.settings = { ...this.settingsService.settings };
    this.customColor = this.settings.accentColor;
  }

  ngOnInit(): void {
    this.settingsService.settings$
      .pipe(takeUntil(this.destroy$))
      .subscribe(settings => {
        this.settings = { ...settings };
        this.customColor = settings.accentColor;
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  selectPresetColor(hex: string): void {
    this.customColor = hex;
    this.settings.accentColor = hex;
    this.settingsService.updateSettings({ accentColor: hex });
  }

  onCustomColorChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.customColor = input.value;
    this.settings.accentColor = input.value;
    this.settingsService.updateSettings({ accentColor: input.value });
  }

  onShowSpoolIdsChange(checked: boolean): void {
    this.settings.showSpoolIds = checked;
    this.settingsService.updateSettings({ showSpoolIds: checked });
  }

  resetToDefaults(): void {
    this.settingsService.resetToDefaults();
  }

  close(): void {
    this.dialogRef.close();
  }

  isSelectedColor(hex: string): boolean {
    return this.settings.accentColor.toLowerCase() === hex.toLowerCase();
  }
}
