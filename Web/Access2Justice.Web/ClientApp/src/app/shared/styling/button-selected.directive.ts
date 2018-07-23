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
    this.button.nativeElement.style.backgroundColor =
      this.button.nativeElement.style.backgroundColor === 'rgb(217, 242, 248)' ? 'transparent' : 'rgb(217, 242, 248)';
  }
}
