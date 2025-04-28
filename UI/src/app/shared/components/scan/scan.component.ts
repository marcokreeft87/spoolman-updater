import {
  Component,
  EventEmitter,
  Output,
  ViewChild,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  NgxScannerQrcodeModule,
  NgxScannerQrcodeComponent,
} from 'ngx-scanner-qrcode';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-camera-scan',
  standalone: true,
  imports: [CommonModule, NgxScannerQrcodeModule, MatSelectModule],
  templateUrl: './scan.component.html',
  styleUrls: ['./scan.component.scss'],
})
export class CameraScanComponent {
  @Output() scanningComplete = new EventEmitter<string>();

  @ViewChild(NgxScannerQrcodeComponent)
  barcodeScanner!: NgxScannerQrcodeComponent;

  barcodeValue: string = '';
  scanning: boolean = false;
  
  cameraId: string | null = null;
  backCameras: MediaDeviceInfo[] = [];

  async ngAfterViewInit() {
    const devices = await navigator.mediaDevices.enumerateDevices();
    const videoDevices = devices.filter(
      (device) => device.kind === 'videoinput'
    );

    // Find cameras that are probably back-facing
    this.backCameras = videoDevices.filter(
      (device) =>
        device.label.toLowerCase().includes('back') ||
        device.label.toLowerCase().includes('rear') ||
        device.label.toLowerCase().includes('environment') ||
        device.label.toLowerCase().includes('outward')
    );

    if (this.backCameras.length > 0) {
      this.cameraId = this.backCameras[0].deviceId; // Default to first back camera
    } else if (videoDevices.length > 0) {
      this.cameraId = videoDevices[0].deviceId; // fallback
    }

    console.log('Back cameras found:', this.backCameras);
  }

  onCameraChange(event: any) {
    this.barcodeScanner.stop(); // Stop the scanner before changing camera

    this.barcodeScanner.playDevice(event.target.value); // Start the scanner with the new camera
    this.cameraId = event.target.value;
  }

  startScanning() {
    this.barcodeScanner.start().subscribe((result) => {
      this.barcodeScanner.devices.subscribe((devices) => {
        const backCamera = devices.filter(
          (device) =>
            device.label.toLowerCase().includes('back') ||
            device.label.toLowerCase().includes('rear')
        );

        if (backCamera && backCamera.length > 1) {
          this.barcodeScanner.playDevice(backCamera[1].deviceId);
        }
        else 
          this.barcodeScanner.playDevice(backCamera[0].deviceId);
      });
    });
  }

  stopScanning() {
    this.barcodeScanner.stop();
  }

  onScanSuccess(scannedResult: any) {
    this.barcodeValue = scannedResult[0].value;

    this.barcodeScanner.stop(); // Stop the scanner after a successful scan

    this.scanningComplete.emit(this.barcodeValue);
  }
}
