import { Directive, ElementRef, HostListener, Input } from '@angular/core';

@Directive({
  selector: '[appButtonSelected]'
})
export class ButtonSelectedDirective {
  @Input() appButtonSelected: string;

  constructor(private button: ElementRef) { }

  @HostListener('click') onClick() {
    this.highlight();
  }

  private highlight() {
    if (this.button.nativeElement.checked) {
      this.button.nativeElement.style.backgroundColor = 'rgb(217, 242, 248)';
      console.log(this.button.nativeElement.checked);
    } else {
      console.log(this.button.nativeElement.checked);
      this.button.nativeElement.style.backgroundColor = 'transparent';
    }
  }
}
