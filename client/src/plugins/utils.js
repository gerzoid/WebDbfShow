export function HideFirstPage()
{
    const els = document.querySelectorAll(".intro");
    els.forEach((el) => {
      el.style.display = "none";
    });
    const els2 = document.querySelectorAll(".shows");
    els2.forEach((el) => {
      el.style.display = "initial";
    });
}