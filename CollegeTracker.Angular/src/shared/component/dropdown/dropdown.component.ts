import { NgForOf } from "@angular/common";
import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from "@angular/core";

export interface DropdownSelectIdItem {
    title: string;
    id: number | null;
}

@Component({
    standalone: true,
    selector: "app-dropdown-select-id",
    templateUrl: "./dropdown.component.html",
    styleUrls: ["./dropdown.component.scss"],
    imports: [NgForOf]
  })
  export class DropdownSelectIdComponent implements OnChanges {

    @Input({ required: true })
    public items: DropdownSelectIdItem[] = [];

    @Input()
    public currentItem: DropdownSelectIdItem | null = null;

    public selectedItem: DropdownSelectIdItem | null = null;

    @Output()
    public itemSelected = new EventEmitter<DropdownSelectIdItem>();

    public ngOnChanges(changes: SimpleChanges): void {
      if (changes['currentItem'] && changes['currentItem'].currentValue && this.selectedItem == null) {
        this.setElement(changes['currentItem'].currentValue);
      }
    }
    public setElement(item: DropdownSelectIdItem) {
        this.itemSelected.emit(item);
        this.selectedItem = item;
    }

    public getInputId() {
      return `dropdown-${this.items[0].title}`
    }
  }
  