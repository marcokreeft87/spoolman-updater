import { Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { SpoolsService } from './shared/service/spoolman.service';
import { HttpClientModule } from '@angular/common/http';
import { TrayService } from './shared/service/tray.service';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { CameraScanComponent } from './shared/components/scan/scan.component';
import { SettingsComponent } from './shared/components/settings/settings.component';
import { SettingsService } from './shared/service/settings.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    HttpClientModule,
    CommonModule,
    MatToolbarModule,
    RouterModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    CameraScanComponent,
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers: [SpoolsService, TrayService],
})
export class AppComponent {
  scanning = false;

  constructor(
    private router: Router,
    private dialog: MatDialog,
    private settingsService: SettingsService // Inject to trigger initialization
  ) {}

  openScanner() {
    this.scanning = !this.scanning;
  }

  closeScanner() {
    this.scanning = false;
  }

  goToScan(barcode: string) {
    this.scanning = false;
    this.router.navigate(['/scan'], { queryParams: { barcode: barcode } });
  }

  openSettings() {
    // Blur any focused element before opening dialog to prevent aria-hidden warning
    if (document.activeElement instanceof HTMLElement) {
      document.activeElement.blur();
    }

    this.dialog.open(SettingsComponent, {
      width: '400px',
      maxWidth: '95vw',
      autoFocus: false,
      restoreFocus: false,  // Don't restore focus to button after close
    });
  }
}
