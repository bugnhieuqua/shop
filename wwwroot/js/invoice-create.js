(() => {
  const form = document.getElementById('invoiceForm');
  if (!form) return;

  const body   = document.getElementById('lineBody');
  const addBtn = document.getElementById('addLine');
  const fmt    = n => new Intl.NumberFormat('vi-VN').format(n) + ' ₫';
  let rowIndex = body.querySelectorAll('.line-row').length;

  function recalc() {
    let subtotal = 0;
    body.querySelectorAll('.line-row').forEach(row => {
      const sel = row.querySelector('.line-row__product');
      const qty = parseInt(row.querySelector('.line-row__qty').value, 10) || 0;
      const opt = sel.selectedOptions[0];
      const price = opt && opt.dataset.price ? Number(opt.dataset.price) : 0;
      const line  = price * qty;

      const unitPriceEl = row.querySelector('[data-role="unit-price"]');
      const lineTotalEl = row.querySelector('[data-role="line-total"]');
      const hiddenInput = row.querySelector('[data-role="unit-price-input"]');

      if (unitPriceEl) unitPriceEl.textContent = fmt(price);
      if (lineTotalEl) lineTotalEl.textContent = fmt(line);
      if (hiddenInput) hiddenInput.value = price;

      subtotal += line;
    });

    const subtotalEl = document.getElementById('subtotal');
    const grandTotalEl = document.getElementById('grandTotal');
    if (subtotalEl) subtotalEl.textContent = fmt(subtotal);
    if (grandTotalEl) grandTotalEl.textContent = fmt(subtotal);
  }

  function renumber() {
    body.querySelectorAll('.line-row').forEach((row, i) => {
      row.dataset.index = i;
      row.querySelectorAll('input, select').forEach(el => {
        el.name = el.name.replace(/Lines\[\d+\]/g, `Lines[${i}]`);
        el.id   = el.id.replace(/Lines_\d+__/g, `Lines_${i}__`);
      });
      const removeBtn = row.querySelector('[data-remove-row]');
      if (removeBtn) {
        removeBtn.setAttribute('aria-label', `Xoá dòng ${i + 1}`);
      }
    });
    rowIndex = body.querySelectorAll('.line-row').length;
  }

  body.addEventListener('input', recalc);
  body.addEventListener('change', recalc);

  body.addEventListener('click', e => {
    const btn = e.target.closest('[data-remove-row]');
    if (!btn) return;
    if (body.querySelectorAll('.line-row').length === 1) {
      alert('Hoá đơn phải có ít nhất 1 sản phẩm.');
      return;
    }
    const row = btn.closest('.line-row');
    if (row) {
      row.remove();
      renumber();
      recalc();
    }
  });

  if (addBtn) {
    addBtn.addEventListener('click', () => {
      const firstRow = body.querySelector('.line-row');
      if (!firstRow) return;
      const newRow = firstRow.cloneNode(true);

      newRow.querySelectorAll('input, select').forEach(el => {
        if (el.type !== 'hidden') {
          el.value = el.tagName === 'SELECT' ? '' : '1';
        }
        el.name = el.name.replace(/Lines\[\d+\]/g, `Lines[${rowIndex}]`);
        el.id   = el.id.replace(/Lines_\d+__/g, `Lines_${rowIndex}__`);
      });

      newRow.querySelectorAll('.form-error').forEach(el => {
        el.textContent = '';
      });

      const removeBtn = newRow.querySelector('[data-remove-row]');
      if (removeBtn) {
        removeBtn.setAttribute('aria-label', `Xoá dòng ${rowIndex + 1}`);
      }

      body.appendChild(newRow);
      rowIndex++;
      renumber();
      recalc();
    });
  }

  // Guard unsaved changes
  let dirty = false;
  form.addEventListener('input', () => { dirty = true; });
  form.addEventListener('submit', () => { dirty = false; });
  window.addEventListener('beforeunload', e => {
    if (!dirty) return;
    e.preventDefault();
    e.returnValue = '';
  });

  recalc();
})();
