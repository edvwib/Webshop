document.querySelectorAll('.productPrice').forEach(priceBtn => {
  priceBtn.addEventListener('mouseover', (e) => {
    e.target.value = 'Add to cart';
  });
  priceBtn.addEventListener('mouseout', (e) => {
    e.target.value = e.target.dataset.productprice;
  });
});

document.querySelectorAll('.productCount').forEach(counter => {
  counter.addEventListener('input', (e) => {
    let hiddenEl = document.querySelector(`[data-controlsproductid="${e.target.dataset.counterproductid}"]`);
    hiddenEl.querySelector('#count').value = e.target.value;
  });
});
