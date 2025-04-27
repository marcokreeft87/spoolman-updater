
import { AfterViewInit, Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxScannerQrcodeModule, NgxScannerQrcodeComponent } from 'ngx-scanner-qrcode';

@Component({
  selector: 'app-camera-scan',
  standalone: true,
  imports: [
    CommonModule,
    NgxScannerQrcodeModule
  ],
  templateUrl: './scan.component.html',
  styleUrls: ['./scan.component.scss'],
})
export class CameraScanComponent {
  @Output() scanningComplete = new EventEmitter<string>();  

  @ViewChild(NgxScannerQrcodeComponent) barcodeScanner!: NgxScannerQrcodeComponent;

  barcodeValue: string = '';
  scanning: boolean = false;

  startScanning() {
    this.barcodeScanner.start().subscribe((result) => {
      this.barcodeScanner.devices.subscribe((devices) => {

        const backCamera = devices.find(device => device.label.toLowerCase().includes('back') || device.label.toLowerCase().includes('rear'));

        if (backCamera) {          
          this.barcodeScanner.playDevice(backCamera.deviceId); 
        }
      });
    });
  }

  stopScanning() {
    this.barcodeScanner.stop();
  }

  onScanSuccess(scannedResult: any) {    
    this.barcodeValue = scannedResult[0].value;

    this.scanningComplete.emit(this.barcodeValue);
  }
}
