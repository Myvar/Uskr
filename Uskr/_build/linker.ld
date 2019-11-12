ENTRY(start)

SECTIONS {
. = 0x0100000;


	
  .text  : {
    code = .;
    *(.text)
    . = ALIGN(4096);
  }

  .data : {
    data = .;
    *(.data)
    . = ALIGN(4096);
  }

  .bss : {
    bss = .;
    *(.bss)
    . = ALIGN(4096);
  }
  
   .heap : {
    heap = .;
    *(.heap)
    . = ALIGN(4096);
  }

  end = .;
}