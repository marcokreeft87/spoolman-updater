import { Component, Host, HostBinding, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { SpoolsService } from './shared/service/spoolman.service';
import { HttpClientModule } from '@angular/common/http';
import { TrayService } from './shared/service/tray.service';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { CameraScanComponent } from './shared/components/scan/scan.component';

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
    CameraScanComponent,
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers: [SpoolsService, TrayService],
})
export class AppComponent {
  @ViewChild(CameraScanComponent)
  scanComponent!: CameraScanComponent;

  scanning = false;

  constructor(private router: Router) {}

  openScanner() {
    this.scanning = !this.scanning;

    if (this.scanning)
      this.scanComponent.startScanning();
    else
      this.scanComponent.stopScanning();
  }

  goToScan(barcode: string) {
    this.scanning = false;
    this.router.navigate(['/scan'], { queryParams: { barcode: barcode } });
  }
}
