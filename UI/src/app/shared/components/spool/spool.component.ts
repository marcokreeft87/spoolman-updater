import { CommonModule } from '@angular/common';
import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { Spool } from '../../models/spool';
import { SettingsService } from '../../service/settings.service';

@Component({
  selector: 'app-spool-item',
  standalone: true,
  imports: [
    CommonModule,
  ],
  templateUrl: './spool.component.html',
  styleUrls: ['./spool.component.scss'],
})
export class SpoolItemComponent implements OnInit, OnDestroy {
  @Input() spool: Spool | undefined;
  showSpoolIds = false;

  private destroy$ = new Subject<void>();

  constructor(private settingsService: SettingsService) {}

  ngOnInit(): void {
    this.settingsService.settings$
      .pipe(takeUntil(this.destroy$))
      .subscribe(settings => {
        this.showSpoolIds = settings.showSpoolIds;
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  getTextColor(hexColor: string): string {
    if (!hexColor) return 'black'; // default
  
    // Parse r, g, b
    const r = parseInt(hexColor.substring(0, 2), 16);
    const g = parseInt(hexColor.substring(2, 4), 16);
    const b = parseInt(hexColor.substring(4, 6), 16);
  
    // Calculate brightness
    const brightness = (r * 299 + g * 587 + b * 114) / 1000;
  
    // Return black for light backgrounds, white for dark backgrounds
    return brightness > 150 ? 'black' : 'white';
  }
}
