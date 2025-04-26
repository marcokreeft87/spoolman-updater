import { Component, Host, HostBinding, ViewEncapsulation } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { SpoolsService } from './shared/service/spoolman.service';
import { HttpClientModule } from '@angular/common/http';
import { TrayService } from './shared/service/tray.service';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { ZXingScannerModule } from '@zxing/ngx-scanner';

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
    ZXingScannerModule
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers: [SpoolsService, TrayService]
})
export class AppComponent {
  scanning = false;

  constructor(private router: Router) {}

  openScanner() {
    this.scanning = true;
  }

  handleScan(barcode: string) {
    this.scanning = false;

    // Navigate with the scanned result as a query param
    this.router.navigate(['/scan'], { queryParams: { barcode: barcode } });
  }
}
