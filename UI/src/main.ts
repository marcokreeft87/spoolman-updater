import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { LOAD_WASM } from 'ngx-scanner-qrcode';

// Load WASM for QR code scanner from local assets
LOAD_WASM('assets/wasm/ngx-scanner-qrcode.wasm').subscribe();

bootstrapApplication(AppComponent, appConfig)
  .catch((err) => console.error(err));
