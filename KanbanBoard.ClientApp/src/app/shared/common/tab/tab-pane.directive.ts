import { Directive, Input, TemplateRef } from "@angular/core";

@Directive({
    selector: '[tabPane]',
    standalone: true
})
export class TabPaneDirective{
     @Input('tabPane') label!: string;
     constructor(public template: TemplateRef<any>) {}
}