import { Component, signal } from '@angular/core';
import { SlideOver } from '../../../shared/common/slide-over/slide-over';

@Component({
  selector: 'app-project',
  imports: [SlideOver],
  templateUrl: './project.html',
  styleUrl: './project.scss',
})
export class Project {

  protected isSlideOverOpen = signal(false);
  toggleSlideOver() {
    this.isSlideOverOpen.update(val => !val);
  }

}
