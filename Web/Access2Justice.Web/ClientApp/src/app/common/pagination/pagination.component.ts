import { Component, Input, EventEmitter, Output } from "@angular/core";

@Component({
  selector: "app-pagination",
  templateUrl: "./pagination.component.html",
  styleUrls: ["./pagination.component.css"]
})
export class PaginationComponent {
  @Input() page: number;
  @Input() count: number;
  @Input() perPage: number;
  @Input() loading: boolean;
  @Input() pagesToShow: number;
  @Output() goPrev = new EventEmitter<boolean>();
  @Output() goNext = new EventEmitter<boolean>();
  @Output() goPage = new EventEmitter<number>();

  constructor() {}

  getMin(): number {
    return this.perPage * this.page - this.perPage + 1;
  }

  getMax(): number {
    let max = this.perPage * this.page;
    if (max > this.count) {
      max = this.count;
    }
    return max;
  }

  onPage(n: number): void {
    this.goPage.emit(n);
    window.scroll(0, 0);
  }

  onPrev(): void {
    this.goPrev.emit(true);
    window.scroll(0, 0);
  }

  onNext(next: boolean): void {
    this.goNext.emit(next);
    window.scroll(0, 0);
  }

  totalPages(): number {
    return Math.ceil(this.count / this.perPage) || 0;
  }

  lastPage(): boolean {
    return this.perPage * this.page >= this.count;
  }

  getPages(): number[] {
    const pagesToDisplay = Math.ceil(this.count / this.perPage);
    const currentPage = this.page || 1;
    const pagesToShow = this.pagesToShow || 2;
    const pages: number[] = [];
    pages.push(currentPage);
    const times = pagesToShow - 1;
    for (let i = 0; i < times; i++) {
      if (pages.length < pagesToShow) {
        if (Math.min.apply(null, pages) > 1) {
          pages.push(Math.min.apply(null, pages) - 1);
        }
      }
      if (pages.length < pagesToShow) {
        if (Math.max.apply(null, pages) < pagesToDisplay) {
          pages.push(Math.max.apply(null, pages) + 1);
        }
      }
    }
    pages.sort((a, b) => a - b);
    return pages;
  }
}
