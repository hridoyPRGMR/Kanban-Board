import { Component, computed, input, output, HostListener, ChangeDetectionStrategy } from '@angular/core';

export type SlideOverWidth = 'sm' | 'md' | 'lg' | 'xl';
export type SlideOverPosition = 'left' | 'right';

@Component({
  selector: 'app-slide-over',
  templateUrl: './slide-over.html',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SlideOver {

  readonly open = input.required<boolean>();
  readonly title = input.required<string>();

  readonly width = input<SlideOverWidth>('md');
  readonly position = input<SlideOverPosition>('right');
  readonly closeOnBackdrop = input<boolean>(true);
  readonly closeOnEscape = input<boolean>(true);

  closed = output<void>();

  protected panelWidth = computed(() => {
    const widths: Record<SlideOverWidth, number> = {
      sm: 300,
      md: 400,
      lg: 500,
      xl: 600,
    };
    return widths[this.width()];
  });

  onBackdropClick() {
    if (this.closeOnBackdrop()) {
      this.handleClose();
    }
  }

  handleClose() {
    this.closed.emit();
  }

  @HostListener('window:keydown.escape')
  onEscape() {
    if (this.open() && this.closeOnEscape()) {
      this.handleClose();
    }
  }
}