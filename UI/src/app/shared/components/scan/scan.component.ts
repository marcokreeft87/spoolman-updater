
import { ZXingScannerModule } from '@zxing/ngx-scanner';
import { BarcodeFormat } from '@zxing/library';
import { AfterViewInit, Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BarcodeScannerLivestreamComponent, BarcodeScannerLivestreamModule } from 'ngx-barcode-scanner';

@Component({
  selector: 'app-camera-scan',
  standalone: true,
  imports: [
    CommonModule,
    BarcodeScannerLivestreamModule 
  ],
  templateUrl: './scan.component.html',
  styleUrls: ['./scan.component.scss'],
})
export class CameraScanComponent {
  @Output() scanningComplete = new EventEmitter<string>();  

  @ViewChild(BarcodeScannerLivestreamComponent)
  barcodeScanner: BarcodeScannerLivestreamComponent = new BarcodeScannerLivestreamComponent();

  barcodeValue: string = '';

  startScanning() {
    this.barcodeScanner.start();
  }

  stopScanning() {
    this.barcodeScanner.stop();
  }

  onValueChanges(result: any) {
    this.barcodeValue = result.codeResult.code;
    alert(this.barcodeValue);

    this.scanningComplete.emit(this.barcodeValue);
  }
}
