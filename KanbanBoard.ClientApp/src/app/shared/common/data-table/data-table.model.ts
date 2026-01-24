import { TemplateRef } from "@angular/core";

export interface TableColumn<T>
{
    key: keyof T;
    header: string;
    sortable?: boolean;
    template?: TemplateRef<{$implicit: T}>;
}