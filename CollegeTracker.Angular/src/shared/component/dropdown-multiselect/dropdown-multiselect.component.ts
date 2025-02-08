import { NgForOf } from "@angular/common";
import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from "@angular/core";

export interface DropdownSelectIdItem {
    title: string;
    id: number;
}

@Component({
    standalone: true,
    selector: "app-dropdown-multiselect-ids",
    templateUrl: "./dropdown-multiselect.component.html",
    styleUrls: ["./dropdown-multiselect.component.scss"],
    imports: [NgForOf]
  })
  export class DropdownMultiSelectIdsComponent implements OnChanges {

    @Input({ required: true })
    public items: DropdownSelectIdItem[] = [];

    @Input()
    public currentItems: DropdownSelectIdItem[] = [];

    public selectedItems: DropdownSelectIdItem[] = [];

    @Output()
    public itemSelected = new EventEmitter<DropdownSelectIdItem[]>();

    public ngOnChanges(changes: SimpleChanges): void {
      if (changes['currentItems'] && changes['currentItems'].currentValue && this.selectedItems.length == 0 && changes['currentItems'].firstChange) {
        changes['currentItems'].currentValue.forEach((element: DropdownSelectIdItem) => {
          this.setElement(element);
        });
      }
    }
    public setElement(item: DropdownSelectIdItem) {
      if (this.selectedItems.map(x => x.id).includes(item.id)) {
        this.selectedItems = this.selectedItems.filter(x => x.id != item.id);
      }
      else {
        this.selectedItems.push(item);
      }
      this.itemSelected.emit(this.selectedItems);
    }

    public isItemSelected(item: DropdownSelectIdItem) {
      return this.selectedItems.map(x => x.id).includes(item.id);
    }

    public getItemsString() {
      if (this.selectedItems.length == 0) return null;
      return this.selectedItems.map(x => x.title).join(', ');
    }
  }
  