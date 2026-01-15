import {
  Component,
  EventEmitter,
  Output,
  ViewChild,
  AfterViewInit,
  OnDestroy,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  NgxScannerQrcodeModule,
  NgxScannerQrcodeComponent,
  ScannerQRCodeConfig,
} from 'ngx-scanner-qrcode';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-camera-scan',
  standalone: true,
  imports: [CommonModule, NgxScannerQrcodeModule, MatSelectModule],
  templateUrl: './scan.component.html',
  styleUrls: ['./scan.component.scss'],
})
export class CameraScanComponent implements AfterViewInit, OnDestroy {
  @Output() scanningComplete = new EventEmitter<string>();

  @ViewChild(NgxScannerQrcodeComponent)
  barcodeScanner!: NgxScannerQrcodeComponent;

  barcodeValue: string = '';
  scanning: boolean = false;
  private scanProcessed: boolean = false;

  cameraId: string | null = null;
  backCameras: MediaDeviceInfo[] = [];
  allCameras: MediaDeviceInfo[] = [];

  // Configuration for better camera quality
  scannerConfig: ScannerQRCodeConfig = {
    fps: 60,  // Higher FPS for faster detection
    vibrate: 300,
    isBeep: true,
    constraints: {
      audio: false,
      video: {
        width: { ideal: 1280 },
        height: { ideal: 720 },
        facingMode: 'environment', // Prefer back camera
      }
    }
  };

  private cameraInitialized = false;

  ngAfterViewInit() {
    // Auto-start scanning when component is mounted
    setTimeout(() => this.startScanning(), 100);
  }

  ngOnDestroy() {
    // Auto-stop scanning when component is destroyed
    this.stopScanning();
  }

  async initializeCamera() {
    if (this.cameraInitialized) return;

    try {
      // Request camera permission first to get proper labels
      const tempStream = await navigator.mediaDevices.getUserMedia({ video: true });

      const devices = await navigator.mediaDevices.enumerateDevices();
      const videoDevices = devices.filter(
        (device) => device.kind === 'videoinput'
      );

      // Stop the temporary stream immediately - we only needed it for permission
      tempStream.getTracks().forEach(track => track.stop());

      this.allCameras = videoDevices;

      // Find cameras that are probably back-facing
      this.backCameras = videoDevices.filter(
        (device) =>
          device.label.toLowerCase().includes('back') ||
          device.label.toLowerCase().includes('rear') ||
          device.label.toLowerCase().includes('environment') ||
          device.label.toLowerCase().includes('outward')
      );

      // If no labeled back cameras found, use all cameras
      if (this.backCameras.length === 0) {
        this.backCameras = videoDevices;
      }

      if (this.backCameras.length > 0) {
        this.cameraId = this.backCameras[0].deviceId;
      }

      this.cameraInitialized = true;
    } catch (error) {
      console.error('Error initializing camera:', error);
    }
  }

  onCameraChange(event: Event) {
    const select = event.target as HTMLSelectElement;
    const deviceId = select.value;

    if (this.barcodeScanner && deviceId) {
      this.barcodeScanner.stop();
      this.cameraId = deviceId;

      // Update config with new device
      this.scannerConfig = {
        ...this.scannerConfig,
        constraints: {
          audio: false,
          video: {
            deviceId: { exact: deviceId },
            width: { ideal: 1920 },
            height: { ideal: 1080 },
          }
        }
      };

      this.barcodeScanner.start().subscribe({
        error: (err) => console.error('Error switching camera:', err)
      });
    }
  }

  async startScanning() {
    if (!this.barcodeScanner) return;

    // Reset flag for new scan session
    this.scanProcessed = false;

    // Initialize camera on first use (requests permission)
    await this.initializeCamera();

    // Update config with camera preference
    if (this.cameraId) {
      this.scannerConfig = {
        ...this.scannerConfig,
        constraints: {
          audio: false,
          video: {
            deviceId: { ideal: this.cameraId },
            width: { ideal: 1920 },
            height: { ideal: 1080 },
          }
        }
      };
    }

    this.barcodeScanner.start().subscribe({
      next: () => {
        this.scanning = true;
      },
      error: (err) => console.error('Error starting scanner:', err)
    });
  }

  stopScanning() {
    if (this.barcodeScanner) {
      this.barcodeScanner.stop();
      this.scanning = false;
    }
  }

  onScanSuccess(event: any) {
    // Prevent duplicate processing
    if (this.scanProcessed) return;
    if (!event || event.length === 0) return;

    const value = event[0]?.value;
    if (!value) return;

    this.scanProcessed = true;
    this.barcodeValue = value;

    // Stop the scanner after a successful scan
    this.barcodeScanner.stop();

    this.scanningComplete.emit(this.barcodeValue);
  }
}
