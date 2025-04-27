
import { ZXingScannerModule } from '@zxing/ngx-scanner';
import { BarcodeFormat } from '@zxing/library';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-camera-scan[scanning]',
  standalone: true,
  imports: [
    CommonModule,
    ZXingScannerModule
  ],
  templateUrl: './scan.component.html',
  styleUrls: ['./scan.component.scss'],
})
export class CameraScanComponent {
  @Input() scanning: boolean = false;

  @Output() scanningComplete = new EventEmitter<string>();

  formats: BarcodeFormat[] = [BarcodeFormat.CODE_128];

  handleScan(barcode: string) {
    this.scanning = false;

    // Navigate with the scanned result as a query param
    //this.router.navigate(['/scan'], { queryParams: { barcode: barcode } });
    this.scanningComplete.emit(barcode);
  }
  
  selectedDevice: MediaDeviceInfo | undefined;

  onCamerasFound(devices: MediaDeviceInfo[]) {
    const rearCamera = devices.find(device => /back|rear|environment/i.test(device.label));
    if (rearCamera) {
      this.selectedDevice = rearCamera;
    } else {
      this.selectedDevice = devices[0]; // fallback
    }
  }

  onPermissionResponse(permission: boolean) {
    console.log('Camera permission granted?', permission);
    if (!permission) {
      alert('Camera permission was denied.');
    }
  }
}
