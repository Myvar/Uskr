BITS 32
global start
start:
  extern _FAAF0DE554FAF3091BE39267E23BF3B8
  mov [_FAAF0DE554FAF3091BE39267E23BF3B8], dword start_of_heap
    mov esp, sys_stack     ; This points the stack to our new stack area
    jmp stublet

; This part MUST be 4byte aligned, so we solve that issue using 'ALIGN 4'
ALIGN 4
mboot:
    ; Multiboot macros to make a few lines later more readable
    MULTIBOOT_PAGE_ALIGN    equ 1<<0
    MULTIBOOT_MEMORY_INFO   equ 1<<1
    MULTIBOOT_AOUT_KLUDGE   equ 1<<16
    MULTIBOOT_HEADER_MAGIC  equ 0x1BADB002
    MULTIBOOT_HEADER_FLAGS  equ MULTIBOOT_PAGE_ALIGN | MULTIBOOT_MEMORY_INFO | MULTIBOOT_AOUT_KLUDGE
    MULTIBOOT_CHECKSUM  equ -(MULTIBOOT_HEADER_MAGIC + MULTIBOOT_HEADER_FLAGS)
    EXTERN code, bss, end

    ; This is the GRUB Multiboot header. A boot signature
    dd MULTIBOOT_HEADER_MAGIC
    dd MULTIBOOT_HEADER_FLAGS
    dd MULTIBOOT_CHECKSUM
    
    ; AOUT kludge - must be physical addresses. Make a note of these:
    ; The linker script fills in the data for these ones!
    dd mboot
    dd code
    dd bss
    dd end
    dd start

; This is an endless loop here. Make a note of this: Later on, we
; will insert an 'extern _main', followed by 'call _main', right
; before the 'jmp $'.
stublet:
extern _Init
call _Init
    extern _{{CALL}}
    call _{{CALL}}
    jmp $

SECTION .text

global _70750F0AFB2995CC120C9F760274C2F9
_70750F0AFB2995CC120C9F760274C2F9:
    push ebp
    mov ebp,esp
    sub esp, 16
    
    mov ax, [ebp+8]
    mov dx, [ebp+12] 
    
    out dx, ax 
    mov eax,  ecx
    
    leave
    ret
    
global _753F891548C9A78650968C6000A46A20
_753F891548C9A78650968C6000A46A20:
    push ebp
    mov ebp,esp
    sub esp, 16
    
    mov al, [ebp+8]
    mov dx, [ebp+12] 
    
    out dx, al 
    
    leave
    ret

global _D271CF90018F771C18C5300730392647
_D271CF90018F771C18C5300730392647:
    push ebp
    mov ebp,esp
    sub esp, 16
    
    mov dx, [ebp+8]
    
    in al, dx 
    
    
    leave
    ret
    
global _idt_load
extern _idtp
_idt_load:
    lidt [_idtp]
    ret


; In just a few pages in this tutorial, we will add our Interrupt
; Service Routines (ISRs) right here!
global _isr0
global _isr1
global _isr2
global _isr3
global _isr4
global _isr5
global _isr6
global _isr7
global _isr8
global _isr9
global _isr10
global _isr11
global _isr12
global _isr13
global _isr14
global _isr15
global _isr16
global _isr17
global _isr18
global _isr19
global _isr20
global _isr21
global _isr22
global _isr23
global _isr24
global _isr25
global _isr26
global _isr27
global _isr28
global _isr29
global _isr30
global _isr31

;  0: Divide By Zero Exception
_isr0:
    cli
    push byte 0
    push byte 0
    jmp _isr_common_stub

;  1: Debug Exception
_isr1:
    cli
    push byte 0
    push byte 1
    jmp _isr_common_stub

;  2: Non Maskable Interrupt Exception
_isr2:
    cli
    push byte 0
    push byte 2
    jmp _isr_common_stub

;  3: Int 3 Exception
_isr3:
    cli
    push byte 0
    push byte 3
    jmp _isr_common_stub

;  4: INTO Exception
_isr4:
    cli
    push byte 0
    push byte 4
    jmp _isr_common_stub

;  5: Out of Bounds Exception
_isr5:
    cli
    push byte 0
    push byte 5
    jmp _isr_common_stub

;  6: Invalid Opcode Exception
_isr6:
    cli
    push byte 0
    push byte 6
    jmp _isr_common_stub

;  7: Coprocessor Not Available Exception
_isr7:
    cli
    push byte 0
    push byte 7
    jmp _isr_common_stub

;  8: Double Fault Exception (With Error Code!)
_isr8:
    cli
    push byte 8
    jmp _isr_common_stub

;  9: Coprocessor Segment Overrun Exception
_isr9:
    cli
    push byte 0
    push byte 9
    jmp _isr_common_stub

; 10: Bad TSS Exception (With Error Code!)
_isr10:
    cli
    push byte 10
    jmp _isr_common_stub

; 11: Segment Not Present Exception (With Error Code!)
_isr11:
    cli
    push byte 11
    jmp _isr_common_stub

; 12: Stack Fault Exception (With Error Code!)
_isr12:
    cli
    push byte 12
    jmp _isr_common_stub

; 13: General Protection Fault Exception (With Error Code!)
_isr13:
    cli
    push byte 13
    jmp _isr_common_stub

; 14: Page Fault Exception (With Error Code!)
_isr14:
    cli
    push byte 14
    jmp _isr_common_stub

; 15: Reserved Exception
_isr15:
    cli
    push byte 0
    push byte 15
    jmp _isr_common_stub

; 16: Floating Point Exception
_isr16:
    cli
    push byte 0
    push byte 16
    jmp _isr_common_stub

; 17: Alignment Check Exception
_isr17:
    cli
    push byte 0
    push byte 17
    jmp _isr_common_stub

; 18: Machine Check Exception
_isr18:
    cli
    push byte 0
    push byte 18
    jmp _isr_common_stub

; 19: Reserved
_isr19:
    cli
    push byte 0
    push byte 19
    jmp _isr_common_stub

; 20: Reserved
_isr20:
    cli
    push byte 0
    push byte 20
    jmp _isr_common_stub

; 21: Reserved
_isr21:
    cli
    push byte 0
    push byte 21
    jmp _isr_common_stub

; 22: Reserved
_isr22:
    cli
    push byte 0
    push byte 22
    jmp _isr_common_stub

; 23: Reserved
_isr23:
    cli
    push byte 0
    push byte 23
    jmp _isr_common_stub

; 24: Reserved
_isr24:
    cli
    push byte 0
    push byte 24
    jmp _isr_common_stub

; 25: Reserved
_isr25:
    cli
    push byte 0
    push byte 25
    jmp _isr_common_stub

; 26: Reserved
_isr26:
    cli
    push byte 0
    push byte 26
    jmp _isr_common_stub

; 27: Reserved
_isr27:
    cli
    push byte 0
    push byte 27
    jmp _isr_common_stub

; 28: Reserved
_isr28:
    cli
    push byte 0
    push byte 28
    jmp _isr_common_stub

; 29: Reserved
_isr29:
    cli
    push byte 0
    push byte 29
    jmp _isr_common_stub

; 30: Reserved
_isr30:
    cli
    push byte 0
    push byte 30
    jmp _isr_common_stub

; 31: Reserved
_isr31:
    cli
    push byte 0
    push byte 31
    jmp _isr_common_stub


; We call a C function in here. We need to let the assembler know
; that '_fault_handler' exists in another file
extern _fault_handler

; This is our common ISR stub. It saves the processor state, sets
; up for kernel mode segments, calls the C-level fault handler,
; and finally restores the stack frame.
_isr_common_stub:
    pusha
    push ds
    push es
    push fs
    push gs
    mov ax, 0x10
    mov ds, ax
    mov es, ax
    mov fs, ax
    mov gs, ax
    mov eax, esp
    push eax
    mov eax, _fault_handler
    call eax
    pop eax
    pop gs
    pop fs
    pop es
    pop ds
    popa
    add esp, 8
    iret

global _irq0
global _irq1
global _irq2
global _irq3
global _irq4
global _irq5
global _irq6
global _irq7
global _irq8
global _irq9
global _irq10
global _irq11
global _irq12
global _irq13
global _irq14
global _irq15

; 32: IRQ0
_irq0:
    cli
    push byte 0
    push byte 32
    jmp _irq_common_stub

; 33: IRQ1
_irq1:
    cli
    push byte 0
    push byte 33
    jmp _irq_common_stub

; 34: IRQ2
_irq2:
    cli
    push byte 0
    push byte 34
    jmp _irq_common_stub

; 35: IRQ3
_irq3:
    cli
    push byte 0
    push byte 35
    jmp _irq_common_stub

; 36: IRQ4
_irq4:
    cli
    push byte 0
    push byte 36
    jmp _irq_common_stub

; 37: IRQ5
_irq5:
    cli
    push byte 0
    push byte 37
    jmp _irq_common_stub

; 38: IRQ6
_irq6:
    cli
    push byte 0
    push byte 38
    jmp _irq_common_stub

; 39: IRQ7
_irq7:
    cli
    push byte 0
    push byte 39
    jmp _irq_common_stub

; 40: IRQ8
_irq8:
    cli
    push byte 0
    push byte 40
    jmp _irq_common_stub

; 41: IRQ9
_irq9:
    cli
    push byte 0
    push byte 41
    jmp _irq_common_stub

; 42: IRQ10
_irq10:
    cli
    push byte 0
    push byte 42
    jmp _irq_common_stub

; 43: IRQ11
_irq11:
    cli
    push byte 0
    push byte 43
    jmp _irq_common_stub

; 44: IRQ12
_irq12:
    cli
    push byte 0
    push byte 44
    jmp _irq_common_stub

; 45: IRQ13
_irq13:
    cli
    push byte 0
    push byte 45
    jmp _irq_common_stub

; 46: IRQ14
_irq14:
    cli
    push byte 0
    push byte 46
    jmp _irq_common_stub

; 47: IRQ15
_irq15:
    cli
    push byte 0
    push byte 47
    jmp _irq_common_stub

extern _irq_handler

_irq_common_stub:
    pusha
    push ds
    push es
    push fs
    push gs

    mov ax, 0x10
    mov ds, ax
    mov es, ax
    mov fs, ax
    mov gs, ax
    mov eax, esp

    push eax
    mov eax, _irq_handler
    call eax
    pop eax

    pop gs
    pop fs
    pop es
    pop ds
    popa
    add esp, 8
    iret

SECTION .bss
    resb 8192               ; This reserves 8KBytes of memory here
sys_stack:
SECTION .heap
 start_of_heap: 
    ;resb 8192               ; This reserves 8KBytes of memory here
    resb 65536               ; This reserves 8KBytes of memory here
